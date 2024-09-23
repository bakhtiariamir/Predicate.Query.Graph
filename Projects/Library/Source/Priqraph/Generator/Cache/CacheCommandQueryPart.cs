using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;

namespace Priqraph.Generator.Cache;

public class CacheCommandQueryPart : CacheQueryPart<CacheParameterCollection>
{
    private Dictionary<string, object> _commandParts;

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


    public ICollection<BaseQueryParameter> Parameters
    {
        get;
    }

    private void SetOptions(QueryOperationType operationType) => OperationType = operationType;

    private CacheCommandQueryPart()
    {
        Parameters = new List<BaseQueryParameter>();
        _commandParts = new Dictionary<string, object>();
    }

    private CacheCommandQueryPart(ICollection<CacheClausePredicate> parameters) : this() => Parameter = new CacheParameterCollection(parameters);

    public static CacheCommandQueryPart Create(ICollection<CacheClausePredicate> parameterCollections) => new(parameterCollections);

    public static CacheCommandQueryPart Merge(QueryOperationType? operationType, params CacheCommandQueryPart[] commandParts)
    {
        //if (commandParts.DistinctBy(item => item.CommandValueType).Count() > 1)
        //    throw new NotSupported(ExceptionCode.DatabaseQueryFilteringGenerator); //todo
        
        var commandPredicates = new List<CacheClausePredicate>();
        CacheCommandQueryPart? cacheCommandPart = null;

        var commands = commandParts.Select(item => item.Parameter.CachePredicates);

        foreach (var columnPropertyCollection in commands)
            if (columnPropertyCollection != null)
                commandPredicates.AddRange(columnPropertyCollection);

        cacheCommandPart = new CacheCommandQueryPart(commandPredicates);

        //var commandType = commandParts.Select(item => item.CommandValueType).First();

        //switch (commandType)
        //{
        //    case CommandValueType.Record:

        //        break;
        //    case CommandValueType.Bulk:

        //        break;
        //}

        if (cacheCommandPart == null)
            throw new NotSupportedOperationException("asd"); //todo

        if (operationType.HasValue)
            cacheCommandPart.SetOptions(operationType.Value);

        cacheCommandPart.SetCommandObject();

        return cacheCommandPart;
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
            case QueryOperationType.GetData:
            default: throw new NotSupportedOperationException(""); // todo
        }
    }

    private void SetInsertQuery()
    {

        if (Parameter.CachePredicates is { Count: >= 1 })
        {
            _commandParts["Values"] = Parameter.CachePredicates;
        }

        //switch (CommandValueType)
        //{
        //    case CommandValueType.Record:
        //        if (Parameter.CachePredicates == null)
        //            throw new NotSupported("asdasd"); //todo



        //        //var columnPropertyInfos = Parameter.CachePredicates.First().ColumnProperties.Select(item => item.PropertySelector);

        //        //var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new System.Exception(); //todo

        //        //var columnList = new List<string>();
        //        //var recordsValue = new List<string>();

        //        //var columnProperties = Parameter.CachePredicates?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<CacheCommandProperty>()).ToArray() ?? throw new System.Exception(); //todo

        //        //var records = Parameter.CachePredicates?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

        //        //var recordValue = new List<Tuple<int, string?>>();
        //        //foreach (var columnProperty in columnProperties)
        //        //{
        //        //    if (columnProperty.PropertySelector?.Key ?? false) continue;
        //        //    if ((columnProperty.PropertySelector?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
        //        //    if ((columnProperty.PropertySelector?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
        //        //    if (!(string.IsNullOrWhiteSpace(columnProperty.PropertySelector?.FunctionName ?? string.Empty))) continue;

        //        //    columnList.Add(columnProperty.PropertySelector?.ColumnName);
        //        //    var dbType = columnProperty.PropertySelector.DataType.GetSqlDbType();
        //        //    if (records?.Length > 1)
        //        //    {
        //        //        var index = 0;
        //        //        foreach (var record in records)
        //        //        {
        //        //            var columnValue = Dynamic.InvokeGet(record, columnProperty.PropertySelector?.Name);

        //        //            var parameterName = $"@{SetParameterName(columnProperty.PropertySelector, index)}";
        //        //            var sqlParameter = new SqlParameter(parameterName, dbType) {
        //        //                Value = columnValue ?? DBNull.Value
        //        //            };
        //        //            SqlParameters.Add(sqlParameter);
        //        //            recordValue.Add(new Tuple<int, string?>(index, parameterName));
        //        //            index += 1;
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        //1 if value is iqueryable object
        //        //        var parameterName = $"@{SetParameterName(columnProperty.PropertySelector, 0)}";
        //        //        var sqlParameter = new SqlParameter(parameterName, dbType) {
        //        //            Value = columnProperty.Value ?? DBNull.Value
        //        //        };
        //        //        SqlParameters.Add(sqlParameter);
        //        //        recordValue.Add(new Tuple<int, string?>(0, parameterName));
        //        //    }
        //        //}

        //        //foreach (var recordValueItem in recordValue.GroupBy(item => item.Item1).Select(item => item.Select(row => row.Item2)))
        //        //    recordsValue.Add($"({string.Join(", ", recordValueItem)})");

        //        //var values = string.Join(", ", recordsValue);
        //        //var columns = string.Join(", ", columnList);

        //        //_commandParts.Add("Selector", selector);
        //        //_commandParts.Add("Columns", columns);
        //        //_commandParts.Add("Values", values);
        //        //if (returnType == ReturnType.Record)
        //        //    _commandParts.Add("result", "DECLARE @ResultId INT = (SELECT @@IDENTITY) ");
        //        //else if (returnType == ReturnType.KeyValue)
        //        //    _commandParts.Add("result", "SELECT @@IDENTITY AS Id");
        //        break;
        //}
    }

    private void SetUpdateQuery()
    {

        if (Parameter.CachePredicates is { Count: >= 1 })
        {
            _commandParts["Values"] = Parameter.CachePredicates;
        }



        //switch (CommandValueType)
        //{
        //    case CommandValueType.Record:
        //        if (Parameter.CachePredicates == null)
        //            throw new NotSupported("asdasd"); //todo

        //        //var columnPropertyInfos = Parameter.CachePredicates.First().ColumnProperties.Select(item => item.PropertySelector);

        //        //var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
        //        //_commandParts.Add("Selector", selector);

        //        //var columnProperties = Parameter.CachePredicates?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<CacheCommandProperty>()).ToArray() ?? throw new System.Exception(); //todo

        //        //var records = Parameter.CachePredicates?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

        //        //if (records.Length > 1)
        //        //{
        //        //    ICollection<Tuple<int, string?>> recordsValue = new List<Tuple<int, string?>>();
        //        //    ICollection<Tuple<int, string>> recordsWhere = new List<Tuple<int, string>>();
        //        //    var index = 0;
        //        //    foreach (var @record in records)
        //        //    {
        //        //        ICollection<string> recordValue = new List<string>();
        //        //        foreach (var columnProperty in columnProperties)
        //        //        {
        //        //            if ((columnProperty.PropertySelector?.ReadOnly ?? true)) continue;
        //        //            if ((columnProperty.PropertySelector?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
        //        //            if ((columnProperty.PropertySelector?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
        //        //            if (!(string.IsNullOrWhiteSpace(columnProperty.PropertySelector?.FunctionName ?? string.Empty))) continue;

        //        //            var dbType = columnProperty.PropertySelector?.DataType.GetSqlDbType();
        //        //            var columnValue = Dynamic.InvokeGet(record, columnProperty.PropertySelector?.Name);
        //        //            var parameterName = $"@{SetParameterName(columnProperty.PropertySelector, index)}";

        //        //            var sqlParameter = new SqlParameter(parameterName, dbType) {
        //        //                Value = columnValue
        //        //            };
        //        //            SqlParameters.Add(sqlParameter);

        //        //            if (columnProperty.PropertySelector?.Key ?? false)
        //        //            {
        //        //                recordsWhere.Add(new Tuple<int, string>(index, columnProperty.PropertySelector.GetParameterPhraseBasedOnSqlDbType(parameterName, (object)columnValue)));
        //        //                continue;
        //        //            }

        //        //            recordValue.Add(columnProperty.PropertySelector.GetParameterPhraseBasedOnSqlDbType(parameterName, (object)columnValue));
        //        //        }

        //        //        recordsValue.Add(new Tuple<int, string?>(index, string.Join(", ", recordValue)));
        //        //        index += 1;
        //        //    }

        //        //    _commandParts.Add("RecordsValue", recordsValue);
        //        //    _commandParts.Add("RecordsWhere", recordsWhere);
        //        //}
        //        //else
        //        //{
        //        //    var valueList = new List<string>();
        //        //    foreach (var columnProperty in columnProperties)
        //        //    {
        //        //        if ((columnProperty.PropertySelector?.AggregateFunctionType ?? AggregateFunctionType.None) != AggregateFunctionType.None) continue;
        //        //        if ((columnProperty.PropertySelector?.RankingFunctionType ?? RankingFunctionType.None) != RankingFunctionType.None) continue;
        //        //        if (!(string.IsNullOrWhiteSpace(columnProperty.PropertySelector?.FunctionName ?? string.Empty))) continue;

        //        //        var dbType = columnProperty.PropertySelector?.DataType.GetSqlDbType();
        //        //        var parameterName = $"@{SetParameterName(columnProperty.PropertySelector, 0)}";

        //        //        var sqlParameter = new SqlParameter(parameterName, dbType) {
        //        //            Value = columnProperty.Value
        //        //        };
        //        //        SqlParameters.Add(sqlParameter);

        //        //        if (columnProperty.PropertySelector?.Key ?? false)
        //        //        {
        //        //            _commandParts["Where"] = columnProperty.PropertySelector.GetParameterPhraseBasedOnSqlDbType(parameterName, columnProperty.Value);
        //        //            if (returnType == ReturnType.Record)
        //        //                _commandParts.Add("result", $"DECLARE @ResultId INT = {parameterName}");
        //        //            continue;
        //        //        }

        //        //        valueList.Add(columnProperty.PropertySelector.GetParameterPhraseBasedOnSqlDbType(parameterName, columnProperty.Value));
        //        //    }

        //        //    var values = string.Join(", ", valueList);
        //        //    _commandParts["Values"] = values;
        //        //}

        //        break;
        //}
    }

    private void SetDeleteQuery()
    {

        if (Parameter.CachePredicates is { Count: >= 1 })
        {
            var keyValueCollection = new List<object>();

            foreach (var parameterKey in Parameter.CachePredicates)
            {
                keyValueCollection.Add(parameterKey.Key);
            }
            _commandParts["Keys"] = keyValueCollection;
        }

        //switch (CommandValueType)
        //{
        //    case CommandValueType.Record:
        //        if (Parameter.CachePredicates == null)
        //            throw new NotSupported("asdasd"); //todo





                //var selector = columnPropertyInfos.First()?.GetSelector() ?? throw new NotSupported("asd"); //todo
                //_commandParts.Add("Selector", selector);

                //var columnProperties = Parameter.CachePredicates?.SelectMany(item => item.ColumnProperties ?? Enumerable.Empty<CacheCommandProperty>()).ToArray() ?? throw new System.Exception(); //todo

                //var primaryKey = columnProperties.FirstOrDefault(item => item.PropertySelector?.Key ?? false) ?? throw new NotSupported("asd"); //todo

                //var primaryKeyColumn = primaryKey?.PropertySelector ?? throw new NotSupported("asd"); //todo
                //var parameterName = BaseSetParameterName(primaryKeyColumn);

                //var records = Parameter.CachePredicates?.SelectMany(item => item.Records ?? Enumerable.Empty<object>()).ToArray();

                //string where;

                //if (records.Length > 1)
                //{
                //    var ids = records.Select(record => Dynamic.InvokeGet(record, primaryKeyColumn.Name)).ToArray() ?? throw new System.Exception(); //todo

                //    foreach (var parameter in SqlParameterHelper.ArrayParameters(parameterName, ids, primaryKeyColumn.DataType))
                //        SqlParameters.Add(parameter);

                //    where = primaryKeyColumn.ArrayParameterNames(parameterName, ids.Length);
                //}
                //else
                //{
                //    where = primaryKeyColumn.GetParameterPhraseBasedOnSqlDbType(parameterName, primaryKey.Value);
                //    var dbType = primaryKeyColumn?.DataType.GetSqlDbType();

                //    var sqlParameter = new SqlParameter(parameterName, dbType) {
                //        Value = primaryKey.Value
                //    };
                //    SqlParameters.Add(sqlParameter);
                //}

                //_commandParts["Where"] = where;

        //        break;
        //}
    }
}

public class CacheParameterCollection
{
    public ICollection<CacheClausePredicate>? CachePredicates
    {
        get;
    }
    public CacheParameterCollection(ICollection<CacheClausePredicate> cachePredicates) => CachePredicates = cachePredicates;
}

public class CacheClausePredicate
{
    public CacheClausePredicate(object key, object o)
    {
        Key = key;
        Object = o;
    }

    public object Key
    {
        get;
    }

    public object Object
    {
        get;
    }
}