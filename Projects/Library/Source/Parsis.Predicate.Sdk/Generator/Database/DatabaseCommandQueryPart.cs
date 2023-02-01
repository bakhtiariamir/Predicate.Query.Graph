using Dynamitey;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Helper;
using System.Data.SqlClient;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabaseCommandQueryPart : DatabaseQueryPart<CommandPredicate>
{
    private string? _text;
    private Dictionary<string, object> _commandParts;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    public Dictionary<string, object> CommandParts
    {
        get => _commandParts;
        set => _commandParts = value;
    }

    public QueryOperationType OperationType
    {
        get;
        private set;
    } = QueryOperationType.Add;

    public CommandValueType CommandValueType
    {
        get;
        private set;
    } = CommandValueType.Record;

    public ICollection<SqlParameter> SqlParameters
    {
        get;
    }

    private void SetOptions(QueryOperationType operationType, CommandValueType commandValueType)
    {
        OperationType = operationType;
        CommandValueType = commandValueType;
    }

    private DatabaseCommandQueryPart()
    {
        SqlParameters = new List<SqlParameter>();
        _commandParts = new Dictionary<string, object>();
    }

    private DatabaseCommandQueryPart(IEnumerable<WhereClause> wherePredicates) : this() => Parameter = new CommandPredicate(wherePredicates);

    private DatabaseCommandQueryPart(ColumnPropertyCollection columnPropertyCollection) : this() => Parameter = new CommandPredicate(new[] {columnPropertyCollection});

    private DatabaseCommandQueryPart(ICollection<ColumnPropertyCollection> columnPropertyCollections) : this() => Parameter = new CommandPredicate(columnPropertyCollections);

    private DatabaseCommandQueryPart(IEnumerable<WhereClause> wherePredicates, ColumnPropertyCollection columnPropertyCollection) : this() => Parameter = new CommandPredicate(wherePredicates, new[] {columnPropertyCollection});

    private DatabaseCommandQueryPart(ColumnProperty columnProperty) : this(new ColumnPropertyCollection(new[] {columnProperty}))
    {
    }

    public static DatabaseCommandQueryPart Create(ColumnPropertyCollection columnPropertyCollection) => new(columnPropertyCollection);

    public static DatabaseCommandQueryPart Create(params ColumnProperty[] columnProperties) => new(new ColumnPropertyCollection(columnProperties));

    public static DatabaseCommandQueryPart Merge(QueryOperationType? operationType, params DatabaseCommandQueryPart[] commandParts)
    {
        if (commandParts.DistinctBy(item => item.CommandValueType).Count() > 1)
            throw new NotSupported(ExceptionCode.DatabaseQueryFilteringGenerator); //todo

        var commandType = commandParts.Select(item => item.CommandValueType).First();
        var commandPredicates = new List<ColumnPropertyCollection>();
        DatabaseCommandQueryPart? databaseCommandPart = null;
        switch (commandType)
        {
            case CommandValueType.Record:
                var commands = commandParts.Select(item => item.Parameter.ColumnPropertyCollections);

                foreach (var columnPropertyCollection in commands)
                    commandPredicates.AddRange(columnPropertyCollection);

                databaseCommandPart = new DatabaseCommandQueryPart(commandPredicates);
                break;
            case CommandValueType.Bulk:

                break;
        }

        if (databaseCommandPart == null)
            throw new NotSupported("asd"); //todo

        if (operationType.HasValue)
            databaseCommandPart.SetOptions(operationType.Value, commandType);

        databaseCommandPart.SetCommandObject();

        return databaseCommandPart;
    }

    private void SetCommandObject()
    {
        switch (OperationType)
        {
            case QueryOperationType.Add:
                SetInsertQuery();
                break;
            case QueryOperationType.Edit:
                SetUpdateQuery();
                break;
            case QueryOperationType.Remove:
                SetDeleteQuery();
                break;
            case QueryOperationType.Merge:
                SetMergeQuery();
                break;
            case QueryOperationType.GetData:
            default: throw new NotSupported(""); // todo
        }
    }

    private void SetInsertQuery()
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter.ColumnPropertyCollections == null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections.First().ColumnProperties.Select(item => item.ColumnPropertyInfo);

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new System.Exception(); //todo

                var columnList = new List<string>();
                var recordsValue = new List<string>();

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                var recordValue = new List<Tuple<int, string?>>();
                foreach (var columnProperty in columnProperties)
                {
                    if ((columnProperty.ColumnPropertyInfo?.IsPrimaryKey ?? false && (columnProperty.ColumnPropertyInfo?.IsIdentity ?? true))) continue;
                    if ((columnProperty.ColumnPropertyInfo?.ReadOnly ?? true)) continue;
                    if ((columnProperty.ColumnPropertyInfo?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                    if ((columnProperty.ColumnPropertyInfo?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                    if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo?.FunctionName ?? string.Empty))) continue;

                    columnList.Add(columnProperty.ColumnPropertyInfo?.ColumnName);
                    var dbType = columnProperty.ColumnPropertyInfo.DataType.GetSqlDbType();
                    if (records?.Length > 1)
                    {
                        var index = 0;
                        foreach (var record in records)
                        {
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo?.Name);
                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";
                            var sqlParameter = new SqlParameter(parameterName, dbType) {
                                Value = columnValue
                            };
                            SqlParameters.Add(sqlParameter);
                            recordValue.Add(new Tuple<int, string?>(index, columnProperty.ColumnPropertyInfo?.DataType.GetParameterStringBasedOnSqlDbType(parameterName, (object)columnValue)));
                            index += 1;
                        }
                    }
                    else
                    {
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";
                        var sqlParameter = new SqlParameter(parameterName, dbType) {
                            Value = columnProperty.Value
                        };
                        SqlParameters.Add(sqlParameter);
                        recordValue.Add(new Tuple<int, string?>(0, columnProperty.ColumnPropertyInfo?.DataType.GetParameterStringBasedOnSqlDbType(parameterName, columnProperty.Value)));
                    }
                }

                foreach (var recordValueItem in recordValue.GroupBy(item => item.Item1).Select(item => item.Select(row => row.Item2)))
                    recordsValue.Add($"({string.Join(", ", recordValueItem)})");

                var values = string.Join(", ", recordsValue);
                var columns = string.Join(", ", columnList);

                _commandParts.Add("Selector", selector);
                _commandParts.Add("Columns", columns);
                _commandParts.Add("Values", values);
                break;
        }
    }

    private void SetUpdateQuery()
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter.ColumnPropertyCollections == null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections.First().ColumnProperties.Select(item => item.ColumnPropertyInfo);

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
                _commandParts.Add("Selector", selector);

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                if (records.Length > 1)
                {
                    ICollection<Tuple<int, string?>> recordsValue = new List<Tuple<int, string?>>();
                    ICollection<Tuple<int, string>> recordsWhere = new List<Tuple<int, string>>();
                    var index = 0;
                    foreach (var @record in records)
                    {
                        ICollection<string> recordValue = new List<string>();
                        foreach (var columnProperty in columnProperties)
                        {
                            if ((columnProperty.ColumnPropertyInfo?.ReadOnly ?? true)) continue;
                            if ((columnProperty.ColumnPropertyInfo?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                            if ((columnProperty.ColumnPropertyInfo?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                            if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo?.FunctionName ?? string.Empty))) continue;

                            var dbType = columnProperty.ColumnPropertyInfo?.DataType.GetSqlDbType();
                            var columnValue = Dynamic.InvokeGet(record, columnProperty.ColumnPropertyInfo?.Name);
                            var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, index)}";

                            var sqlParameter = new SqlParameter(parameterName, dbType) {
                                Value = columnValue
                            };
                            SqlParameters.Add(sqlParameter);

                            if (columnProperty.ColumnPropertyInfo?.IsPrimaryKey ?? false)
                            {
                                recordsWhere.Add(new Tuple<int, string>(index, columnProperty.ColumnPropertyInfo.GetParameterPhraseBasedOnSqlDbType(parameterName, (object)columnValue)));
                                continue;
                            }

                            recordValue.Add(columnProperty.ColumnPropertyInfo.GetParameterPhraseBasedOnSqlDbType(parameterName, (object)columnValue));
                        }

                        recordsValue.Add(new Tuple<int, string?>(index, string.Join(", ", recordValue)));
                        index += 1;
                    }

                    _commandParts.Add("RecordsValue", recordsValue);
                    _commandParts.Add("RecordsWhere", recordsWhere);
                }
                else
                {
                    var valueList = new List<string>();
                    foreach (var columnProperty in columnProperties)
                    {
                        if ((columnProperty.ColumnPropertyInfo?.ReadOnly ?? true)) continue;
                        if ((columnProperty.ColumnPropertyInfo?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
                        if ((columnProperty.ColumnPropertyInfo?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
                        if (!(string.IsNullOrWhiteSpace(columnProperty.ColumnPropertyInfo?.FunctionName ?? string.Empty))) continue;

                        var dbType = columnProperty.ColumnPropertyInfo?.DataType.GetSqlDbType();
                        var parameterName = $"@{SetParameterName(columnProperty.ColumnPropertyInfo, 0)}";

                        var sqlParameter = new SqlParameter(parameterName, dbType) {
                            Value = columnProperty.Value
                        };
                        SqlParameters.Add(sqlParameter);

                        if (columnProperty.ColumnPropertyInfo?.IsPrimaryKey ?? false)
                        {
                            _commandParts["Where"] = columnProperty.ColumnPropertyInfo.GetParameterPhraseBasedOnSqlDbType(parameterName, columnProperty.Value);
                            continue;
                        }

                        valueList.Add(columnProperty.ColumnPropertyInfo.GetParameterPhraseBasedOnSqlDbType(parameterName, columnProperty.Value));
                    }

                    var values = string.Join(", ", valueList);
                    _commandParts["Values"] = values;
                }

                break;
        }
    }

    private void SetDeleteQuery()
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:
                if (Parameter.ColumnPropertyCollections == null)
                    throw new NotSupported("asdasd"); //todo

                var columnPropertyInfos = Parameter.ColumnPropertyCollections.First().ColumnProperties.Select(item => item.ColumnPropertyInfo);

                var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
                _commandParts.Add("Selector", selector);

                var columnProperties = Parameter.ColumnPropertyCollections?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<ColumnProperty>()).ToArray() ?? throw new System.Exception(); //todo

                var primaryKey = columnProperties.FirstOrDefault(item => item.ColumnPropertyInfo?.IsPrimaryKey ?? false) ?? throw new NotSupported("asd"); //todo

                var primaryKeyColumn = primaryKey?.ColumnPropertyInfo ?? throw new NotSupported("asd"); //todo
                var parameterName = BaseSetParameterName(primaryKeyColumn);

                var records = Parameter.ColumnPropertyCollections?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                string where;

                if (records.Length > 1)
                {
                    var ids = records.Select(record => Dynamic.InvokeGet(record, primaryKeyColumn.Name)).ToArray() ?? throw new System.Exception(); //todo

                    foreach (var parameter in SqlParameterHelper.ArrayParameters(parameterName, ids, primaryKeyColumn.DataType))
                        SqlParameters.Add(parameter);

                    where = primaryKeyColumn.ArrayParameterNames(parameterName, ids.Length);
                }
                else
                {
                    where = primaryKeyColumn.GetParameterPhraseBasedOnSqlDbType(parameterName, primaryKey.Value);
                    var dbType = primaryKeyColumn?.DataType.GetSqlDbType();

                    var sqlParameter = new SqlParameter(parameterName, dbType) {
                        Value = primaryKey.Value
                    };
                    SqlParameters.Add(sqlParameter);
                }

                _commandParts["Where"] = where;

                break;
        }
    }

    private void SetMergeQuery()
    {
        switch (CommandValueType)
        {
            case CommandValueType.Record:

                break;
        }
    }

    private static string BaseSetParameterName(IColumnPropertyInfo item) => $"P_{item.GetCombinedAlias()}";

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"{BaseSetParameterName(item)}_{index ?? 0}";
}

public class CommandPredicate
{
    private IEnumerable<WhereClause>? _wherePredicates;

    public ICollection<ColumnPropertyCollection>? ColumnPropertyCollections
    {
        get;
    }

    public CommandPredicate(IEnumerable<WhereClause> wherePredicates) => _wherePredicates = wherePredicates;

    public CommandPredicate(ICollection<ColumnPropertyCollection> columnPropertyCollections) => ColumnPropertyCollections = columnPropertyCollections;

    public CommandPredicate(IEnumerable<WhereClause> wherePredicates, ICollection<ColumnPropertyCollection> columnPropertyCollections)
    {
        ColumnPropertyCollections = columnPropertyCollections;
        _wherePredicates = wherePredicates;
    }
}

public class ColumnPropertyCollection
{
    public ICollection<ColumnProperty>? ColumnProperties
    {
        get;
    }

    public IEnumerable<object>? Records
    {
        get;
        private set;
    }

    public ColumnPropertyCollection(ICollection<ColumnProperty> columnProperties) => ColumnProperties = columnProperties;

    public ColumnPropertyCollection(IEnumerable<object>? records) => Records = records;

    public void SetData(IEnumerable<object>? records) => Records = records;

    public void AddColumn(ColumnProperty columnProperty) => ColumnProperties?.Add(columnProperty);
}

public class ColumnProperty
{
    public IColumnPropertyInfo? ColumnPropertyInfo
    {
        get;
    }

    public object? Value
    {
        get;
        private set;
    }

    public ColumnProperty(IColumnPropertyInfo columnPropertyInfo) => ColumnPropertyInfo = columnPropertyInfo;

    public ColumnProperty(object value) => Value = value;

    public void SetValue(object? value) => Value = value;
}
