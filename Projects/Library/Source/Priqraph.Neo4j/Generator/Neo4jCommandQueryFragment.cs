using System.Data;
using Dynamitey;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using System.Data.SqlClient;
using Priqraph.Generator;
using Priqraph.Neo4j.Extensions;

namespace Priqraph.Neo4j.Generator;
public class Neo4jCommandQueryFragment : DatabaseCommandQueryFragment
{
    public Neo4jQueryOperationType OperationType
    {
        get;
        set;
    }

    public List<Neo4jParameter> Neo4jParameters
    {
        get;
        set;
    }
    
    private void SetOptions(Neo4jQueryOperationType operationType, CommandValueType commandValueType)
    {
        OperationType = operationType;
        CommandValueType = commandValueType;
    }

    private Neo4jCommandQueryFragment()
    {
        Neo4jParameters = new List<Neo4jParameter>();
        CommandParts = new Dictionary<string, object>();
    }

    private Neo4jCommandQueryFragment(ColumnPropertyCollection columnPropertyCollection) : this()
    {
        Parameter = new CommandProperty(new[] { columnPropertyCollection });
    }

    private Neo4jCommandQueryFragment(ICollection<ColumnPropertyCollection> columnPropertyCollections) : this()
    {
        Parameter = new CommandProperty(columnPropertyCollections);
    }

    public static Neo4jCommandQueryFragment Create(ColumnPropertyCollection columnPropertyCollection) => new(columnPropertyCollection);

    public static Neo4jCommandQueryFragment Create(params ColumnProperty[] columnProperties) => new(new ColumnPropertyCollection(columnProperties));

    public static Neo4jCommandQueryFragment Merge(Neo4jQueryOperationType? operationType, ReturnType returnType = ReturnType.None, params Neo4jCommandQueryFragment[] commandParts)
    {
        if (commandParts.DistinctBy(item => item.CommandValueType).Count() > 1)
            throw new NotSupportedOperationException(ExceptionCode.DatabaseQueryFilteringGenerator); //todo

        var commandType = commandParts.Select(item => item.CommandValueType).First();
        var commandPredicates = new List<ColumnPropertyCollection>();
        Neo4jCommandQueryFragment? databaseCommandPart = null;
        switch (commandType)
        {
            case CommandValueType.Record:
                var commands = commandParts.Where(item => item.Parameter?.ColumnPropertyCollections != null).Select(item => item.Parameter!.ColumnPropertyCollections).ToArray();

                foreach (var columnPropertyCollection in commands)
                    commandPredicates.AddRange(columnPropertyCollection!);

                databaseCommandPart = new Neo4jCommandQueryFragment(commandPredicates);
                break;
            case CommandValueType.Bulk:

                break;
        }

        if (databaseCommandPart == null)
            throw new NotSupportedOperationException("asd"); //todo

        if (operationType.HasValue)
            databaseCommandPart.SetOptions(operationType.Value, commandType);

        databaseCommandPart.SetCommandObject(returnType);

        return databaseCommandPart;
    }

    private void SetCommandObject(ReturnType returnType)
    {
        switch (OperationType)
        {
            case Neo4jQueryOperationType.InsertNode:
                SetInsertQuery(returnType);
                break;
            case Neo4jQueryOperationType.InsertLeaf:
                SetUpdateQuery(returnType);
                break;
            case Neo4jQueryOperationType.InsertNodes:
                SetDeleteQuery();
                break;
            case Neo4jQueryOperationType.InsertRelation:
                SetMergeQuery(returnType);
                break;
            case Neo4jQueryOperationType.FindNode:
            default: throw new NotSupportedOperationException(""); // todo
        }
    }

    private void SetInsertQuery(ReturnType returnType)
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter?.ColumnPropertyCollections is null)
                    throw new NotSupportedOperationException("asdasd"); //todo

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
                    var dataType = columnProperty.ColumnPropertyInfo.DataType();
                    if (records?.Length > 1)
                    {
                        var index = 0;
                        foreach (var record in records)
                        {
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo.Name);

                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";
                            var sqlParameter = new Neo4jParameter(parameterName, columnValue, dataType);
                            Neo4jParameters.Add(sqlParameter);
                            recordValue.Add(new Tuple<int, string?>(index, parameterName));
                            index += 1;
                        }
                    }
                    else
                    {
                        //1 if value is iqueryable object
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";
                        var sqlParameter = new Neo4jParameter(parameterName, columnProperty.Value, dataType);
                        Neo4jParameters.Add(sqlParameter);
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
                    throw new NotSupportedOperationException("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections?.First()?.ColumnProperties?.Select(item => item.ColumnPropertyInfo);
                if (columnPropertyInfos == null)
                    return;

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupportedOperationException("asd"); //todo
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

                            var dataType = columnProperty.ColumnPropertyInfo.DataType;
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo.Name);
                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";
                            var sqlParameter = new Neo4jParameter(parameterName, columnValue, dataType);
                            Neo4jParameters.Add(sqlParameter);
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

                        var dbType = columnProperty.ColumnPropertyInfo.DataType;
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";
                        var sqlParameter = new Neo4jParameter(parameterName, columnProperty.Value ?? DBNull.Value, dbType);
                        Neo4jParameters.Add(sqlParameter);

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
                    throw new NotSupportedOperationException("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections?.First()?.ColumnProperties?.Select(item => item.ColumnPropertyInfo);
                if (columnPropertyInfos == null)
                    return;

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupportedOperationException("asd"); //todo
                CommandParts.Add("Selector", selector);

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var primaryKey = columnProperties.FirstOrDefault(item => item.ColumnPropertyInfo?.Key ?? false) ?? throw new NotSupportedOperationException("asd"); //todo

                var primaryKeyColumn = primaryKey?.ColumnPropertyInfo ?? throw new NotSupportedOperationException("asd"); //todo
                var parameterName = BaseSetParameterName(primaryKeyColumn);

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                string where;

                if (records is { Length: > 0 })
                {
                    var ids = records.Select(record => Dynamic.InvokeGet(record, primaryKeyColumn.Name)).ToArray() ?? throw new System.Exception(); //todo

                    foreach (var parameter in ParameterHelper.ArrayParameters(parameterName, ids, primaryKeyColumn.DataType))
                        Neo4jParameters.Add(parameter);

                    where = primaryKeyColumn.ArrayParameterNames(parameterName, ids.Length);
                }
                else
                {
                    where = primaryKeyColumn.ParameterPhrase(parameterName);
                    var dbType = primaryKeyColumn?.DataType ?? ColumnDataType.Object;

                    var parameter = new Neo4jParameter(parameterName, primaryKey.Value, dbType);
                    Neo4jParameters.Add(parameter);
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