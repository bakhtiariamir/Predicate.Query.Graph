using Dynamitey;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using Priqraph.Query.Builders;
using Priqraph.Sql.Extensions;
using System.Data.SqlClient;

namespace Priqraph.Sql.Generator;
public class CommandQueryFragment : DatabaseCommandQueryFragment
{
    private void SetOptions(QueryOperationType operationType, CommandValueType commandValueType)
    {
        OperationType = operationType;
        CommandValueType = commandValueType;
    }

    private CommandQueryFragment()
    {
        SqlParameters = new List<SqlParameter>();
        CommandParts = new Dictionary<string, object>();
    }

    private CommandQueryFragment(ColumnPropertyCollection columnPropertyCollection) : this()
    {
        Parameter = new CommandProperty(new[] { columnPropertyCollection });
    }

    private CommandQueryFragment(ICollection<ColumnPropertyCollection> columnPropertyCollections) : this()
    {
        Parameter = new CommandProperty(columnPropertyCollections);
    }

    public static CommandQueryFragment Create(ColumnPropertyCollection columnPropertyCollection) => new(columnPropertyCollection);

    public static CommandQueryFragment Create(params ColumnProperty[] columnProperties) => new(new ColumnPropertyCollection(columnProperties));

    public static CommandQueryFragment Merge(QueryOperationType? operationType, ReturnType returnType = ReturnType.None, params CommandQueryFragment[] commandParts)
    {
        if (commandParts.DistinctBy(item => item.CommandValueType).Count() > 1)
            throw new NotSupported(ExceptionCode.DatabaseQueryFilteringGenerator); //todo

        var commandType = commandParts.Select(item => item.CommandValueType).First();
        var commandPredicates = new List<ColumnPropertyCollection>();
        CommandQueryFragment? databaseCommandPart = null;
        switch (commandType)
        {
            case CommandValueType.Record:
                var commands = commandParts.Where(item => item.Parameter?.ColumnPropertyCollections != null).Select(item => item.Parameter!.ColumnPropertyCollections).ToArray();

                foreach (var columnPropertyCollection in commands)
                    commandPredicates.AddRange(columnPropertyCollection!);

                databaseCommandPart = new CommandQueryFragment(commandPredicates);
                break;
            case CommandValueType.Bulk:

                break;
        }

        if (databaseCommandPart == null)
            throw new NotSupported("asd"); //todo

        if (operationType.HasValue)
            databaseCommandPart.SetOptions(operationType.Value, commandType);

        databaseCommandPart.SetCommandObject(returnType);

        return databaseCommandPart;
    }

    private void SetCommandObject(ReturnType returnType)
    {
        switch (OperationType)
        {
            case QueryOperationType.Add:
                SetInsertQuery(returnType);
                break;
            case QueryOperationType.Edit:
                SetUpdateQuery(returnType);
                break;
            case QueryOperationType.Remove:
                SetDeleteQuery();
                break;
            case QueryOperationType.Merge:
                SetMergeQuery(returnType);
                break;
            case QueryOperationType.GetData:
            default: throw new NotSupported(""); // todo
        }
    }

    private void SetInsertQuery(ReturnType returnType)
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter?.ColumnPropertyCollections is null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections?.First()?.ColumnProperties?.Select(item => item.ColumnPropertyInfo);
                if (columnPropertyInfos == null)
                    return;

                var selector = columnPropertyInfos.First()!.GetSelector() ?? throw new System.Exception(); //todo
                var columnList = new List<string>();
                var recordsValue = new List<string>();

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                var recordValue = new List<Tuple<int, string?>>();
                foreach (var columnProperty in columnProperties)
                {
                    if (columnProperty.ColumnPropertyInfo == null) continue;
                    if (columnProperty.ColumnPropertyInfo.Key) continue;
                    if (columnProperty.ColumnPropertyInfo.FieldType == DatabaseFieldType.Related) continue;
                    if ((columnProperty.ColumnPropertyInfo.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                    if ((columnProperty.ColumnPropertyInfo.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                    if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo.FunctionName ?? string.Empty))) continue;

                    columnList.Add(columnProperty.ColumnPropertyInfo.ColumnName);
                    var dbType = columnProperty.ColumnPropertyInfo.DataType.SqlDbType();
                    if (records?.Length > 1)
                    {
                        var index = 0;
                        foreach (var record in records)
                        {
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo.Name);

                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";
                            var sqlParameter = new SqlParameter(parameterName, dbType)
                            {
                                Value = columnValue ?? DBNull.Value
                            };
                            SqlParameters.Add(sqlParameter);
                            recordValue.Add(new Tuple<int, string?>(index, parameterName));
                            index += 1;
                        }
                    }
                    else
                    {
                        //1 if value is iqueryable object
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";
                        var sqlParameter = new SqlParameter(parameterName, dbType)
                        {
                            Value = columnProperty.Value ?? DBNull.Value
                        };
                        SqlParameters.Add(sqlParameter);
                        recordValue.Add(new Tuple<int, string?>(0, parameterName));
                    }
                }

                foreach (var recordValueItem in recordValue.GroupBy(item => item.Item1).Select(item => item.Select(row => row.Item2)))
                    recordsValue.Add($"({string.Join(", ", recordValueItem)})");

                var values = string.Join(", ", recordsValue);
                var columns = string.Join(", ", columnList);

                CommandParts.Add("Selector", selector);
                CommandParts.Add("Columns", columns);
                CommandParts.Add("Values", values);
                if (returnType == ReturnType.Record)
                    CommandParts.Add("result", "DECLARE @ResultId INT = (SELECT @@IDENTITY) ");
                else if (returnType == ReturnType.Key)
                    CommandParts.Add("result", "SELECT @@IDENTITY AS Id");

                break;
        }
    }

    private void SetUpdateQuery(ReturnType returnType)
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter?.ColumnPropertyCollections is null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections?.First()?.ColumnProperties?.Select(item => item.ColumnPropertyInfo);
                if (columnPropertyInfos == null)
                    return;

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
                CommandParts.Add("Selector", selector);

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                if (records is { Length: > 0 })
                {
                    ICollection<Tuple<int, string?>> recordsValue = new List<Tuple<int, string?>>();
                    ICollection<Tuple<int, string>> recordsWhere = new List<Tuple<int, string>>();
                    var index = 0;
                    foreach (var @record in records)
                    {
                        ICollection<string> recordValue = new List<string>();
                        foreach (var columnProperty in columnProperties)
                        {
                            if (columnProperty.ColumnPropertyInfo is null) continue;
                            if (columnProperty.ColumnPropertyInfo.ReadOnly) continue;
                            if (columnProperty.ColumnPropertyInfo.FieldType == DatabaseFieldType.Related) continue;
                            if ((columnProperty.ColumnPropertyInfo.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                            if ((columnProperty.ColumnPropertyInfo.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                            if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo.FunctionName ?? string.Empty))) continue;

                            var dbType = columnProperty.ColumnPropertyInfo.DataType.SqlDbType();
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo.Name);
                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";
                            var sqlParameter = new SqlParameter(parameterName, dbType)
                            {
                                Value = columnValue ?? DBNull.Value
                            };
                            SqlParameters.Add(sqlParameter);

                            if (columnProperty.ColumnPropertyInfo.Key)
                            {
                                recordsWhere.Add(new Tuple<int, string>(index, columnProperty.ColumnPropertyInfo.ParameterPhrase(parameterName)));
                                continue;
                            }

                            recordValue.Add(columnProperty.ColumnPropertyInfo.ParameterPhrase(parameterName));
                        }

                        recordsValue.Add(new Tuple<int, string?>(index, string.Join(", ", recordValue)));
                        index += 1;
                    }

                    CommandParts.Add("RecordsValue", recordsValue);
                    CommandParts.Add("RecordsWhere", recordsWhere);
                }
                else
                {
                    var valueList = new List<string>();
                    foreach (var columnProperty in columnProperties)
                    {
                        if (columnProperty.ColumnPropertyInfo is null) continue;
                        if (columnProperty.ColumnPropertyInfo.FieldType == DatabaseFieldType.Related) continue;
                        if ((columnProperty.ColumnPropertyInfo.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                        if ((columnProperty.ColumnPropertyInfo.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                        if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo.FunctionName ?? string.Empty))) continue;

                        var dbType = columnProperty.ColumnPropertyInfo.DataType.SqlDbType();
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";
                        var sqlParameter = new SqlParameter(parameterName, dbType)
                        {
                            Value = columnProperty.Value ?? DBNull.Value
                        };
                        SqlParameters.Add(sqlParameter);

                        if (columnProperty.ColumnPropertyInfo.Key)
                        {
                            CommandParts["Where"] = columnProperty.ColumnPropertyInfo.ParameterPhrase(parameterName);
                            if (returnType == ReturnType.Record)
                                CommandParts.Add("result", $"DECLARE @ResultId INT = {parameterName}");
                            continue;
                        }

                        valueList.Add(columnProperty.ColumnPropertyInfo.ParameterPhrase(parameterName));
                    }

                    var values = string.Join(", ", valueList);
                    CommandParts["Values"] = values;
                }

                break;
        }
    }

    private void SetDeleteQuery()
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter?.ColumnPropertyCollections is null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections?.First()?.ColumnProperties?.Select(item => item.ColumnPropertyInfo);
                if (columnPropertyInfos == null)
                    return;

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
                CommandParts.Add("Selector", selector);

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var primaryKey = columnProperties.FirstOrDefault(item => item.ColumnPropertyInfo?.Key ?? false) ?? throw new NotSupported("asd"); //todo

                var primaryKeyColumn = primaryKey?.ColumnPropertyInfo ?? throw new NotSupported("asd"); //todo
                var parameterName = BaseSetParameterName(primaryKeyColumn);

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                string where;

                if (records is { Length: > 0 })
                {
                    var ids = records.Select(record => Dynamic.InvokeGet(record, primaryKeyColumn.Name)).ToArray() ?? throw new System.Exception(); //todo

                    foreach (var parameter in ParameterHelper.ArrayParameters(parameterName, ids, primaryKeyColumn.DataType))
                        SqlParameters.Add(parameter);

                    where = primaryKeyColumn.ArrayParameterNames(parameterName, ids.Length);
                }
                else
                {
                    where = primaryKeyColumn.ParameterPhrase(parameterName);
                    var dbType = primaryKeyColumn?.DataType.SqlDbType();

                    var sqlParameter = new SqlParameter(parameterName, dbType)
                    {
                        Value = primaryKey.Value
                    };
                    SqlParameters.Add(sqlParameter);
                }

                CommandParts["Where"] = where;

                break;
        }
    }

    private void SetMergeQuery(ReturnType returnType)
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:

                break;
        }
    }

    private static string BaseSetParameterName(IColumnPropertyInfo item) => $"{item.Name}";

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => (index == null || index == 0) ? $"{BaseSetParameterName(item)}" : $"{BaseSetParameterName(item)}_{index ?? 0}";
}