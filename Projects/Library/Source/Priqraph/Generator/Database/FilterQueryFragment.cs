using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Helper;
using Priqraph.Query.Builders;
using Priqraph.Setup;
using System.Data;
using System.Data.SqlClient;
using static Priqraph.Query.Builders.ReturnType;

namespace Priqraph.Generator.Database;

public class FilterQueryFragment : QueryFragment<FilterClause>
{
    private string? _text;

    public QuerySetting? QuerySetting
    {
        get;
        private set;
    }

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private FilterQueryFragment(FilterClause? parameter)
    {
        Parameter = parameter;
    }

    public static FilterQueryFragment Create(FilterClause? property = null) => new(property);

    public void SetText(ReturnType returnType = None, FilterClause? returnClause = null) =>
        _text = returnType switch
        {
            Record => ReturnResultRecordWhereClause(returnClause ?? throw new ArgumentNullException($"object prameter key can not be null.")),
            _ or None => SetWhereClauseText(Parameter)
        };

    private static string ReturnResultRecordWhereClause(FilterClause returnClause) => $"{returnClause.ColumnPropertyInfo?.GetSelector()}.[{returnClause.ColumnPropertyInfo?.ColumnName}] = @ResultId";

    public static ICollection<IColumnPropertyInfo> GetColumnProperties(FilterClause parameter)
    {
        var columnProperties = new List<IColumnPropertyInfo>();
        switch (parameter)
        {
            case { PartType: PartType.ColumnInfo }:
                columnProperties.Add(parameter.ColumnPropertyInfo!);
                break;
            case { PartType: PartType.WhereClause }:
                columnProperties.AddRange((GetColumnProperties(parameter.Left!)));
                columnProperties.AddRange((GetColumnProperties(parameter.Right!)));
                break;
        }

        return columnProperties;
    }

    public static ICollection<FilterClause> GetHavingClause(FilterClause parameter)
    {
        var columnProperties = new List<FilterClause>();
        if (parameter.Left == null || parameter.Right == null) return columnProperties;

        var left = parameter.Left ?? throw new System.Exception("asd");
        var right = parameter.Right ?? throw new System.Exception("asd");

        if (left.PartType == PartType.ColumnInfo && left.ColumnPropertyInfo != null)
        {
            if (left.ClauseType == ClauseType.Having && left.ColumnPropertyInfo.WindowPartitionColumns?.Length == 0)
            {
                left.SetValue(right.Value);
                left.SetOperator(parameter.Operator);

                columnProperties.Add(left);
            }
        }
        else if (left.Operator is not ConditionOperatorType.IsNotNull or ConditionOperatorType.IsNull)
            columnProperties.AddRange((GetHavingClause(left)));

        if (parameter.Right.ColumnPropertyInfo != null)
        {
            if (right.ClauseType == ClauseType.Having)
            {
                right.SetValue(left.Value);
                right.SetOperator(parameter.Operator);

                columnProperties.Add(right);
            }
        }
        else if (right.Operator is not ConditionOperatorType.IsNotNull or ConditionOperatorType.IsNull)
            columnProperties.AddRange((GetHavingClause(right)));

        return columnProperties;
    }

    public static IEnumerable<SqlParameter>? GetParameters(FilterClause? parameter, QuerySetting setting)
    {
        if (parameter == null) yield break;

        var whereClause = ReduceWhereClause(parameter);
        if (whereClause is null) yield break;

        switch (whereClause.PartType)
        {
            case PartType.WhereClause:
                SqlParameter sqlParameter;
                if (whereClause.Left?.PartType == PartType.ColumnInfo)
                {
                    if (whereClause.Operator is not ConditionOperatorType.IsNotNull and not ConditionOperatorType.IsNull)
                    {
                        sqlParameter = GetParameters(whereClause.Left, setting)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
                        var valueParameter = GetParameters(whereClause.Right, setting)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
                        sqlParameter.ParameterName = valueParameter.ParameterName;
                        if (whereClause.Right?.ValueType?.IsArray ?? false)
                        {
                            sqlParameter.SqlDbType = SqlDbType.Structured;
                            sqlParameter.Value = valueParameter.Value;
                            sqlParameter.TypeName = valueParameter.TypeName;
                        }
                        else
                            sqlParameter.Value = valueParameter.Value;

                        yield return sqlParameter;
                    }
                }
                else
                {
                    var leftParameters = GetParameters(whereClause.Left, setting);
                    if (leftParameters != null)
                        foreach (var leftParameter in leftParameters)
                            yield return leftParameter;

                    var rightParameters = GetParameters(whereClause.Right, setting);
                    if (rightParameters != null)
                        foreach (var rightParameter in rightParameters)
                            yield return rightParameter;
                }

                break;
            case PartType.ParameterInfo:

                string parameterName;
                var typeParam = "";
                if (whereClause.ValueType?.IsArray ?? false)
                {
                    parameterName = SetParameterName(whereClause, true);
                    var userDefinedTables = setting?.Database?.QueryOptions?.SelectOption?.UserDefinedTables?.ToArray() ?? Array.Empty<UserDefinedTable>();
                    var elementType = whereClause.ValueType?.GetElementType();
                    if (elementType == typeof(int))
                    {
                        if (userDefinedTables?.FirstOrDefault(item => item.Key == "Int") != null)
                        {
                            typeParam = userDefinedTables?.FirstOrDefault(item => item.Key == "Int")?.Type;

                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = CreateIntValueDataTable((IEnumerable<int>)parameter.Value!)
                            };
                        }
                        else
                        {
                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = (IEnumerable<object>)parameter.Value!
                            };
                        }
                    }
                    else if (elementType == typeof(long))
                    {
                        if (userDefinedTables?.FirstOrDefault(item => item.Key == "Bigint") != null)
                        {
                            typeParam = userDefinedTables?.FirstOrDefault(item => item.Key == "Bigint")?.Type;

                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = (IEnumerable<long>)parameter.Value!
                            };
                        }
                        else
                        {
                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = (IEnumerable<object>)parameter.Value!
                            };
                        }
                    }
                    else if (elementType == typeof(string))
                    {
                        if (userDefinedTables?.FirstOrDefault(item => item.Key == "String") != null)
                        {
                            typeParam = userDefinedTables?.FirstOrDefault(item => item.Key == "String")?.Type;

                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = (IEnumerable<string>)parameter.Value!
                            };
                        }
                        else
                        {
                            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                            {
                                TypeName = typeParam,
                                Value = (IEnumerable<object>)parameter.Value!
                            };
                        }
                    }
                    else
                    {
                        sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
                        {
                            TypeName = typeParam,
                            Value = (IEnumerable<object>)parameter.Value!
                        };
                    }
                }
                else
                {
                    // if (whereClause.ColumnPropertyInfo!.Name != whereClause.ParameterName)
                    // {
                    //     parameterName = SetParameterName(whereClause);
                    // }
                    // else
                    // {
                    //     //Todo : 228
                    //     var columnName = whereClause.ColumnPropertyInfo.Name;
                    //     var dataSet = whereClause.ColumnPropertyInfo.DataSet;
                    //     parameterName = $"{dataSet}{columnName}";
                    // }
                    if (whereClause.ColumnPropertyInfo == null)
                    {
                        parameterName = SetParameterName(whereClause);
                    }
                    else
                    {
                        var columnName = whereClause.ColumnPropertyInfo.Name;
                        var dataSet = whereClause.ColumnPropertyInfo.DataSet;
                        parameterName = $"{dataSet}{columnName}";
                    }

                    sqlParameter = new SqlParameter
                    {
                        ParameterName = parameterName,
                        Value = parameter.Value
                    };
                }

                yield return sqlParameter;
                break;
            case PartType.ColumnInfo:
                var dbType = whereClause.ColumnPropertyInfo?.DataType.SqlDbType();
                sqlParameter = new SqlParameter(null, dbType);
                yield return sqlParameter;
                break;
            default:
                yield break;
        }
    }

    private string? SetWhereClauseText(FilterClause? parameter)
    {
        if (parameter == null) return null;

        var whereClause = ReduceWhereClause(parameter);
        if (whereClause != null)
        {
            switch (whereClause.PartType)
            {
                case PartType.ColumnInfo:
                    return SetColumnName(whereClause.ColumnPropertyInfo!);
                case PartType.WhereClause:
                    var wherePart = whereClause;
                    if (wherePart.Left is not null)
                    {
                        var left = SetWhereClauseText(wherePart.Left);
                        string? right;
                        var isString = false;
                        var isArray = false;
                        if (wherePart.Right is null && wherePart.Operator != ConditionOperatorType.None)
                            throw new NotFound(whereClause.ToString()!, whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);

                        if (wherePart.Operator is ConditionOperatorType.IsNull or ConditionOperatorType.IsNotNull)
                        {
                            right = null;
                        }
                        else
                        {
                            right = SetWhereClauseText(wherePart.Right);
                            isArray = wherePart.Right?.ValueType?.IsArray ?? false;
                            isString = wherePart.Left.ColumnPropertyInfo != null && new[] { ColumnDataType.Char, ColumnDataType.String }.Contains(wherePart.Left.ColumnPropertyInfo.DataType);
                        }

                        return GenerateClausePhrase(left, wherePart.Operator, right, isString, isArray);
                    }
                    else
                        throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
                case PartType.ParameterInfo:

                    if (whereClause.ValueType?.IsArray ?? false)
                    {
                        var userDefinedTables = QuerySetting?.Database?.QueryOptions?.SelectOption?.UserDefinedTables;
                        var elementType = whereClause.ValueType.GetElementType();
                        if (elementType == typeof(int))
                        {
                            if (userDefinedTables?.FirstOrDefault(item => item.Key == "Int") != null)
                            {
                                var parameterName = $"@{SetParameterName(whereClause, true)}";
                                return $"SELECT [Value] FROM {parameterName}";
                            }

                            if (whereClause.Value is IEnumerable<int> enumerableInt)
                                return string.Join(", ", enumerableInt);
                        }
                        else if (elementType == typeof(long))
                        {
                            if (userDefinedTables?.FirstOrDefault(item => item.Key == "Bigint") != null)
                            {
                                var parameterName = $"@{SetParameterName(whereClause, true)}";
                                return $"SELECT [Value] FROM {parameterName}";
                            }

                            if (whereClause.Value is IEnumerable<long> enumerableInt)
                                return string.Join(", ", enumerableInt);
                        }
                        else if (elementType == typeof(string))
                        {
                            if (userDefinedTables?.FirstOrDefault(item => item.Key == "String") != null)
                            {
                                var parameterName = $"@{SetParameterName(whereClause, true)}";
                                return $"SELECT [Value] FROM {parameterName}";
                            }

                            if (whereClause.Value is IEnumerable<string> enumerableString)
                                return string.Join(", ", $"N'{enumerableString}'");
                        }
                        //ToDo : Complete all types
                    }

                    else if (string.IsNullOrWhiteSpace(whereClause.ParameterName))
                    {

                        return $"@{whereClause.ParameterName}";
                    }
                    else
                    {
                        if (whereClause.ColumnPropertyInfo == null)
                        {
                            return $"@{SetParameterName(whereClause)}";
                        }

                        var columnName = whereClause.ColumnPropertyInfo.Name;
                        var dataSet = whereClause.ColumnPropertyInfo.DataSet;
                        return $"@{dataSet}{columnName}";
                    }


                    return $"@{SetParameterName(whereClause)}";

                default:
                    throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
            }
        }

        return null;
    }

    private static FilterClause? ReduceWhereClause(FilterClause? parameter)
    {
        if (parameter == null) return null;
        switch (parameter.PartType)
        {
            case PartType.ColumnInfo when parameter.ColumnPropertyInfo != null:
                {
                    if (parameter.ClauseType != ClauseType.Having)
                        return parameter;

                    return null;
                }
            case PartType.ParameterInfo:
                return parameter;
            case PartType.WhereClause:
                var leftParameter = ReduceWhereClause(parameter.Left);
                var rightParameter = ReduceWhereClause(parameter.Right);

                if (leftParameter != null && rightParameter == null && leftParameter.PartType != PartType.ParameterInfo)
                    return new FilterClause(leftParameter, null, ConditionOperatorType.None, parameter.ClauseType);

                if (rightParameter != null && leftParameter == null && rightParameter.PartType != PartType.ParameterInfo)
                    return new FilterClause(rightParameter, null, ConditionOperatorType.None, parameter.ClauseType);

                if (leftParameter != null && rightParameter != null)
                    return new FilterClause(leftParameter, rightParameter, parameter.Operator, parameter.ClauseType);
                return null;
        }

        return null;
    }

    private static string GenerateClausePhrase(string left, ConditionOperatorType operatorType, string? right, bool isString, bool isArray = false) =>
        operatorType switch
        {
            ConditionOperatorType.Equal => $"{left} = {right}",
            ConditionOperatorType.NotEqual => $"{left} <> {right}",
            ConditionOperatorType.Like => $"{left} LIKE {right}",
            ConditionOperatorType.NotLike => $"{left} NOT LIKE {right}",
            ConditionOperatorType.LeftLike => $"{left} LIKE %{right}",
            ConditionOperatorType.NotLeftLike => $"{left} NOT LIKE %{right}",
            ConditionOperatorType.RightLike => $"{left} LIKE {right}%",
            ConditionOperatorType.NotRightLike => $"{left} NOT LIKE {right}%",
            ConditionOperatorType.GreaterThan => $"{left} > {right}",
            ConditionOperatorType.GreaterThanEqual => $"{left} >= {right}",
            ConditionOperatorType.LessThan => $"{left} < {right}",
            ConditionOperatorType.LessThanEqual => $"{left} <= {right}",
            ConditionOperatorType.In => $"{left} IN ({right})",
            ConditionOperatorType.Contains => isArray ? $"{left} IN ({right})" : isString ? $"{left} LIKE {right}" : throw new NotSupported(left, ExceptionCode.DatabaseQueryFilteringGenerator),
            ConditionOperatorType.NotIn => $"{left} NOT IN ({right})",
            ConditionOperatorType.IsNull => $"{left} IS NULL",
            ConditionOperatorType.IsNotNull => $"{left} IS NOT NULL",
            ConditionOperatorType.Or => right == null ? $"({right})" : $"({left} OR {right})",
            ConditionOperatorType.And => right == null ? $"({right})" : $"({left} AND {right})",
            ConditionOperatorType.None => $"({left})",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new NotSupported(left, ExceptionCode.DatabaseQueryFilteringGenerator)
        };

    //ToDo : add these methods in helper for use by all QueryPart
    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private static string SetParameterName(FilterClause item, bool isArray = false) => item.ParameterName + (isArray ? "s" : "") + (item.Index > 0 ? $"_{item.Index}" : "");

    public void ReduceParameter(FilterClause? parameter = null, Dictionary<string, int>? parameterIndex = null)
    {
        parameterIndex ??= new Dictionary<string, int>();
        parameter ??= Parameter;

        switch (parameter.Left)
        {
            case { PartType: PartType.ParameterInfo } when parameter?.Left.ParameterName is null:
                throw new ArgumentNullException($"Parameter can not be null for {parameter?.ColumnPropertyInfo?.ColumnName}.");
            case { PartType: PartType.ParameterInfo }:
                {
                    SetParameterIndex(parameter.Left, parameterIndex);
                    parameter?.Left.SetIndex(parameterIndex[parameter?.Left.ParameterName!]);
                    break;
                }
            case { PartType: PartType.WhereClause }:
                ReduceParameter(parameter.Left);
                break;
        }

        if (parameter?.Right is { ParameterName: { } })
        {
            switch (parameter?.Right)
            {
                case { PartType: PartType.ParameterInfo } when parameter?.Right.ParameterName is null:
                    throw new ArgumentNullException($"Parameter can not be null for {parameter?.ColumnPropertyInfo?.ColumnName}.");
                case { PartType: PartType.ParameterInfo }:
                    {
                        SetParameterIndex(parameter.Right, parameterIndex);
                        parameter?.Right.SetIndex(parameterIndex[parameter?.Right.ParameterName!]);
                        break;
                    }
                case { PartType: PartType.WhereClause }:
                    ReduceParameter(parameter.Right);
                    break;
            }
        }
    }

    public void SetQuerySetting(QuerySetting setting) => QuerySetting = setting;
    private static void SetParameterIndex(FilterClause parameter, Dictionary<string, int> parameterIndex)
    {
        if (parameterIndex.ContainsKey(parameter?.ParameterName!))
        {
            parameterIndex[parameter?.ParameterName!] = ++parameterIndex[parameter?.ParameterName!];
        }
        else
            parameterIndex.Add(parameter?.ParameterName!, 0);
    }

    private static DataTable CreateIntValueDataTable(IEnumerable<int> intValues)
    {
        var table = new DataTable();
        table.Columns.Add("Value", typeof(int));
        foreach (long intValue in intValues)
        {
            table.Rows.Add(intValue);
        }
        return table;
    }
}

public class FilterClause
{
    public FilterClause? Left
    {
        get;
    }

    public FilterClause? Right
    {
        get;
    }

    public ConditionOperatorType Operator
    {
        get;
        private set;
    }

    public IColumnPropertyInfo? ColumnPropertyInfo
    {
        get;
        private set;
    }

    public object? Value
    {
        get;
        private set;
    }

    public Type? ValueType
    {
        get;
    }

    public ClauseType ClauseType
    {
        get;
    }

    public PartType PartType
    {
        get;
    }

    public int Index
    {
        get;
        private set;
    } = 0;

    public string? ParameterName
    {
        get;
    }

    public FilterClause(FilterClause left, FilterClause? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where, string? parameterName = null)
    {
        Left = left;
        Right = right;
        Operator = @operator;
        ParameterName = parameterName;
        ClauseType = clauseType;
        PartType = PartType.WhereClause;
    }

    public FilterClause(IColumnPropertyInfo columnPropertyInfo, object? value = null, ConditionOperatorType condition = ConditionOperatorType.None, PartType partType = PartType.ColumnInfo, ClauseType clauseType = ClauseType.Where, string? parameterName = null, Type? valueType = null)
    {
        ColumnPropertyInfo = columnPropertyInfo;
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = clauseType;
        Operator = condition;
        Value = value;
        PartType = partType;
    }

    public FilterClause(object? value = null, Type? valueType = null, string? parameterName = null)
    {
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = ClauseType.None;
        Value = value;
        PartType = PartType.ParameterInfo;
    }

    public static FilterClause CreateWhereClause(FilterClause left, FilterClause? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where) => new(left, right, @operator, clauseType);

    public static FilterClause CreateParameterClause(object? value, Type? valueType, string? parameterName) => new(value, valueType, parameterName);

    public void SetOperator(ConditionOperatorType operatorType) => Operator = operatorType;

    public void SetValue(object? value) => Value = value;

    public void SetIndex(int index) => Index = index;

    public void SetParameterColumnInfo(IColumnPropertyInfo columnPropertyInfo) => ColumnPropertyInfo = columnPropertyInfo;
}

public enum ClauseType
{
    None = 0,
    Where = 1,
    Having = 2,
}

public enum PartType
{
    ColumnInfo = 1,
    WhereClause = 2,
    ParameterInfo = 3
}

public class ValueDetail
{
    public Type? Type
    {
        get;
        set;
    }

    public bool IsGeneric
    {
        get;
        set;
    }

    public bool IsArray
    {
        get;
        set;
    }
}
