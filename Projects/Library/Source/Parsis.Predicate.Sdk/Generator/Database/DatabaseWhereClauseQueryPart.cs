using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Helper;
using System.Data.SqlClient;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabaseWhereClauseQueryPart : DatabaseQueryPart<WhereClause>
{
    private int _index = 0;
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private DatabaseWhereClauseQueryPart(WhereClause parameter) => Parameter = parameter;

    public static DatabaseWhereClauseQueryPart Create(WhereClause property) => new(property);

    public void SetText() => _text = SetWhereClauseText(Parameter);

    public static ICollection<IColumnPropertyInfo> GetColumnProperties(WhereClause parameter)
    {
        var columnProperties = new List<IColumnPropertyInfo>();
        switch (parameter)
        {
            case {PartType: PartType.ColumnInfo}:
                columnProperties.Add(parameter.ColumnPropertyInfo);
                break;
            case {PartType: PartType.WhereClause}:
                columnProperties.AddRange((GetColumnProperties(parameter.Left)));
                columnProperties.AddRange((GetColumnProperties(parameter.Right)));
                break;
        }

        return columnProperties;
    }

    public static ICollection<WhereClause> GeHavingClause(WhereClause parameter)
    {
        var columnProperties = new List<WhereClause>();
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
        else if (left.Operator is not ConditionOperatorType.NotIsNull or ConditionOperatorType.IsNull)
            columnProperties.AddRange((GeHavingClause(left)));

        if (parameter.Right.ColumnPropertyInfo != null)
        {
            if (right.ClauseType == ClauseType.Having)
            {
                right.SetValue(left.Value);
                right.SetOperator(parameter.Operator);

                columnProperties.Add(right);
            }
        }
        else if (right.Operator is not ConditionOperatorType.NotIsNull or ConditionOperatorType.IsNull)
            columnProperties.AddRange((GeHavingClause(right)));

        return columnProperties;
    }

    public static IEnumerable<SqlParameter>? GetParameters(WhereClause? parameter)
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
                    if (whereClause.Operator is not ConditionOperatorType.NotIsNull and not ConditionOperatorType.IsNull)
                    {
                        sqlParameter = GetParameters(whereClause.Left)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
                        var valueParameter = GetParameters(whereClause.Right)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
                        sqlParameter.ParameterName = !string.IsNullOrWhiteSpace(valueParameter.ParameterName) ? valueParameter.ParameterName : $"@{SetParameterName(whereClause.Left?.ColumnPropertyInfo, whereClause.Index)}";
                        sqlParameter.Value = valueParameter.Value;
                        yield return sqlParameter;
                    }
                }
                else
                {
                    var leftParameters = GetParameters(whereClause.Left);
                    if (leftParameters != null)
                        foreach (var leftParameter in leftParameters)
                            yield return leftParameter;

                    var rightParameters = GetParameters(whereClause.Right);
                    if (rightParameters != null)
                        foreach (var rightParameter in rightParameters)
                            yield return rightParameter;
                }

                break;
            case PartType.ParameterInfo:
                sqlParameter = new SqlParameter {
                    ParameterName = $"@{whereClause.ParameterName}",
                    Value = parameter.Value
                };
                yield return sqlParameter;
                break;
            case PartType.ColumnInfo:
                var dbType = whereClause.ColumnPropertyInfo?.DataType.GetSqlDbType();
                sqlParameter = new SqlParameter(null, dbType);
                yield return sqlParameter;
                break;
            default:
                yield break;
        }
    }

    private static string? SetWhereClauseText(WhereClause? parameter)
    {
        if (parameter == null) return null;

        var whereClause = ReduceWhereClause(parameter);
        if (whereClause != null)
        {
            switch (whereClause.PartType)
            {
                case PartType.ColumnInfo:
                    return SetColumnName(whereClause.ColumnPropertyInfo);
                case PartType.WhereClause:
                    var wherePart = whereClause;
                    if (wherePart.Left is not null)
                    {
                        var left = SetWhereClauseText(wherePart.Left);
                        string? right = null;
                        var isString = false;
                        if (wherePart.Right is null && wherePart.Operator != ConditionOperatorType.None)
                            throw new NotFound(whereClause.ToString(), whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);

                        if (wherePart.Operator is ConditionOperatorType.IsNull or ConditionOperatorType.NotIsNull)
                        {
                            right = null;
                        }
                        else
                        {
                            right = SetWhereClauseText(wherePart.Right);
                            isString = wherePart.Left.ColumnPropertyInfo != null && new[] {ColumnDataType.Char, ColumnDataType.String}.Contains(wherePart.Left.ColumnPropertyInfo.DataType);
                        }

                        return GenerateClausePhrase(left, wherePart.Operator, right, isString);
                    }
                    else
                        throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
                case PartType.ParameterInfo:

                    if (!string.IsNullOrWhiteSpace(whereClause.ParameterName))
                        return !string.IsNullOrWhiteSpace(whereClause.ParameterName) ? $"@{whereClause.ParameterName}" : $"@{SetParameterName(whereClause.ColumnPropertyInfo, whereClause.Index)}";

                    return string.Empty;
                default:
                    throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
            }
        }

        return null;
    }

    private static WhereClause? ReduceWhereClause(WhereClause? parameter)
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
                    return new WhereClause(leftParameter, null, ConditionOperatorType.None, parameter.ClauseType);

                if (rightParameter != null && leftParameter == null && rightParameter.PartType != PartType.ParameterInfo)
                    return new WhereClause(rightParameter, null, ConditionOperatorType.None, parameter.ClauseType);

                if (leftParameter != null && rightParameter != null)
                    return new WhereClause(leftParameter, rightParameter, parameter.Operator, parameter.ClauseType);
                return null;
        }

        return null;
    }

    private static string GenerateClausePhrase(string left, ConditionOperatorType operatorType, string? right, bool isString)
    {
        var startQuote = isString ? "N'" : string.Empty;
        var endQuote = isString ? "'" : string.Empty;
        return operatorType switch {
            ConditionOperatorType.Equal => $"{left} = {startQuote}{right}{endQuote}",
            ConditionOperatorType.NotEqual => $"{left} <> {startQuote}{right}{endQuote}",
            ConditionOperatorType.Like => $"{left} LIKE {startQuote}{right}{endQuote}",
            ConditionOperatorType.NotLike => $"{left} NOT LIKE {startQuote}{right}{endQuote}",
            ConditionOperatorType.LeftLike => $"{left} like {startQuote}%{right}{endQuote}",
            ConditionOperatorType.NotLeftLike => $"{left} like {startQuote}%{right}{endQuote}",
            ConditionOperatorType.RightLike => $"{left} like {startQuote}{right}%{endQuote}",
            ConditionOperatorType.NotRightLike => $"{left} like {startQuote}{right}%{endQuote}",
            ConditionOperatorType.GreaterThan => $"{left} > {right}",
            ConditionOperatorType.GreaterThanEqual => $"{left} >= {right}",
            ConditionOperatorType.LessThan => $"{left} < {right}",
            ConditionOperatorType.LessThanEqual => $"{left} <= {right}",
            ConditionOperatorType.In => $"{left} IN ({right})",
            ConditionOperatorType.NotIn => $"{left} NOT IN ({right})",
            ConditionOperatorType.IsNull => $"{left} IS NULL",
            ConditionOperatorType.NotIsNull => $"{left} IS NOT NULL",
            ConditionOperatorType.Or => right == null ? $"({right})" : $"({left} OR {right})",
            ConditionOperatorType.And => right == null ? $"({right})" : $"({left} AND {right})",
            ConditionOperatorType.None => $"({left})",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new NotSupported(left, ExceptionCode.DatabaseQueryFilteringGenerator)
        };
    }

    //ToDo : add these methods in helper for use by all QueryPart
    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"P_{item.GetCombinedAlias()}_{index ?? 0}";

    public void ReduceParameter(WhereClause? parameter = null)
    {
        parameter ??= Parameter;
        if (parameter.Left is {PartType: PartType.ParameterInfo})
        {
            parameter.Left.SetIndex(_index++);
        }
        else if (parameter.Left is {PartType: PartType.WhereClause})
        {
            ReduceParameter(parameter.Left);
        }

        if (parameter.Right is {PartType: PartType.ParameterInfo})
        {
            parameter.Right.SetIndex(_index++);
        }
        else if (parameter.Right is {PartType: PartType.WhereClause})
        {
            ReduceParameter(parameter.Right);
        }
    }
}

public class WhereClause
{
    public WhereClause? Left
    {
        get;
    }

    public WhereClause? Right
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

    public WhereClause(WhereClause left, WhereClause? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where, string? parameterName = null)
    {
        Left = left;
        Right = right;
        Operator = @operator;
        ParameterName = parameterName;
        ClauseType = clauseType;
        PartType = PartType.WhereClause;
    }

    public WhereClause(IColumnPropertyInfo columnPropertyInfo, object? value = null, ConditionOperatorType condition = ConditionOperatorType.None, PartType partType = PartType.ColumnInfo, ClauseType clauseType = ClauseType.Where, string? parameterName = null, Type? valueType = null)
    {
        ColumnPropertyInfo = columnPropertyInfo;
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = clauseType;
        Operator = condition;
        Value = value;
        PartType = partType;
    }

    public WhereClause(object? value = null, Type? valueType = null, string? parameterName = null)
    {
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = ClauseType.None;
        Value = value;
        PartType = PartType.ParameterInfo;
    }

    public static WhereClause CreateWhereClause(WhereClause left, WhereClause? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where) => new(left, right, @operator, clauseType);

    public static WhereClause CreateParameterClause(object? value, Type? valueType, string? parameterName) => new(value, valueType, parameterName);

    public void SetOperator(ConditionOperatorType operatorType) => Operator = operatorType;

    public void SetValue(object? value) => Value = value;

    public void SetIndex(int index) => Index = index;
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
