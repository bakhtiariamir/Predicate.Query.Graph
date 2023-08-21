using System.Linq.Expressions;

namespace Priqraph.Generator.Cache;

public class CacheWhereClauseQueryPart : CacheQueryPart<CacheWhereClause>
{
    private CacheWhereClauseQueryPart(CacheWhereClause parameter) => Parameter = parameter;

    public static CacheWhereClauseQueryPart Create(LambdaExpression predicate) => new(new CacheWhereClause(predicate));
    //public static CacheWhereClauseQueryPart Create(WhereClause property) => new(property);

    //private static string ReturnResultRecordWhereClause(WhereClause returnClause) => $"{returnClause.ColumnPropertyInfo?.GetSelector()}.[{returnClause.ColumnPropertyInfo?.ColumnName}] = @ResultId";

    //public static ICollection<IColumnPropertyInfo> GetColumnProperties(WhereClause parameter)
    //{
    //    var columnProperties = new List<IColumnPropertyInfo>();
    //    switch (parameter)
    //    {
    //        case { PartType: PartType.ColumnInfo }:
    //            columnProperties.Add(parameter.ColumnPropertyInfo);
    //            break;
    //        case { PartType: PartType.WhereClause }:
    //            columnProperties.AddRange((GetColumnProperties(parameter.Left)));
    //            columnProperties.AddRange((GetColumnProperties(parameter.Right)));
    //            break;
    //    }

    //    return columnProperties;
    //}

    //public static ICollection<WhereClause> GetHavingClause(WhereClause parameter)
    //{
    //    var columnProperties = new List<WhereClause>();
    //    if (parameter.Left == null || parameter.Right == null) return columnProperties;

    //    var left = parameter.Left ?? throw new System.Exception("asd");
    //    var right = parameter.Right ?? throw new System.Exception("asd");

    //    if (left.PartType == PartType.ColumnInfo && left.ColumnPropertyInfo != null)
    //    {
    //        if (left.ClauseType == ClauseType.Having && left.ColumnPropertyInfo.WindowPartitionColumns?.Length == 0)
    //        {
    //            left.SetValue(right.Value);
    //            left.SetOperator(parameter.Operator);

    //            columnProperties.Add(left);
    //        }
    //    }
    //    else if (left.Operator is not ConditionOperatorType.IsNotNull or ConditionOperatorType.IsNull)
    //        columnProperties.AddRange((GetHavingClause(left)));

    //    if (parameter.Right.ColumnPropertyInfo != null)
    //    {
    //        if (right.ClauseType == ClauseType.Having)
    //        {
    //            right.SetValue(left.Value);
    //            right.SetOperator(parameter.Operator);

    //            columnProperties.Add(right);
    //        }
    //    }
    //    else if (right.Operator is not ConditionOperatorType.IsNotNull or ConditionOperatorType.IsNull)
    //        columnProperties.AddRange((GetHavingClause(right)));

    //    return columnProperties;
    //}

    //public static IEnumerable<SqlParameter>? GetParameters(WhereClause? parameter)
    //{
    //    if (parameter == null) yield break;

    //    var whereClause = ReduceWhereClause(parameter);
    //    if (whereClause is null) yield break;

    //    switch (whereClause.PartType)
    //    {
    //        case PartType.WhereClause:
    //            SqlParameter sqlParameter;
    //            if (whereClause.Left?.PartType == PartType.ColumnInfo)
    //            {
    //                if (whereClause.Operator is not ConditionOperatorType.IsNotNull and not ConditionOperatorType.IsNull)
    //                {
    //                    if (whereClause.Right is { PartType: PartType.ParameterInfo } && (whereClause.Right.ValueType?.IsArray ?? false))
    //                        yield break;

    //                    sqlParameter = GetParameters(whereClause.Left)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
    //                    var valueParameter = GetParameters(whereClause.Right)?.FirstOrDefault() ?? throw new ArgumentNullException(); //todo
    //                    sqlParameter.ParameterName = valueParameter.ParameterName;
    //                    sqlParameter.Value = valueParameter.Value;
    //                    yield return sqlParameter;
    //                }
    //            }
    //            else
    //            {
    //                var leftParameters = GetParameters(whereClause.Left);
    //                if (leftParameters != null)
    //                    foreach (var leftParameter in leftParameters)
    //                        yield return leftParameter;

    //                var rightParameters = GetParameters(whereClause.Right);
    //                if (rightParameters != null)
    //                    foreach (var rightParameter in rightParameters)
    //                        yield return rightParameter;
    //            }

    //            break;
    //        case PartType.ParameterInfo:
    //            sqlParameter = new SqlParameter
    //            {
    //                ParameterName = SetParameterName(whereClause),
    //                Value = parameter.Value
    //            };
    //            yield return sqlParameter;
    //            break;
    //        case PartType.ColumnInfo:
    //            var dbType = whereClause.ColumnPropertyInfo?.DataType.GetSqlDbType();
    //            sqlParameter = new SqlParameter(null, dbType);
    //            yield return sqlParameter;
    //            break;
    //        default:
    //            yield break;
    //    }
    //}

    //private static string? SetWhereClauseText(WhereClause? parameter)
    //{
    //    if (parameter == null) return null;

    //    var whereClause = ReduceWhereClause(parameter);
    //    if (whereClause != null)
    //    {
    //        switch (whereClause.PartType)
    //        {
    //            case PartType.ColumnInfo:
    //                return SetColumnName(whereClause.ColumnPropertyInfo);
    //            case PartType.WhereClause:
    //                var wherePart = whereClause;
    //                if (wherePart.Left is not null)
    //                {
    //                    var left = SetWhereClauseText(wherePart.Left);
    //                    string? right = null;
    //                    var isString = false;
    //                    var isArray = false;
    //                    if (wherePart.Right is null && wherePart.Operator != ConditionOperatorType.None)
    //                        throw new NotFound(whereClause.ToString(), whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);

    //                    if (wherePart.Operator is ConditionOperatorType.IsNull or ConditionOperatorType.IsNotNull)
    //                    {
    //                        right = null;
    //                    }
    //                    else
    //                    {
    //                        right = SetWhereClauseText(wherePart.Right);
    //                        isArray = wherePart.Right?.ValueType?.IsArray ?? false;
    //                        isString = wherePart.Left.ColumnPropertyInfo != null && new[] { ColumnDataType.Char, ColumnDataType.String }.Contains(wherePart.Left.ColumnPropertyInfo.DataType);
    //                    }

    //                    return GenerateClausePhrase(left, wherePart.Operator, right, isString, isArray);
    //                }
    //                else
    //                    throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
    //            case PartType.ParameterInfo:

    //                if (whereClause.ValueType?.IsArray ?? false)
    //                {
    //                    if (whereClause.Value is IEnumerable<int> enumerableInt)
    //                        return string.Join(", ", enumerableInt);
    //                }

    //                else if (string.IsNullOrWhiteSpace(whereClause.ParameterName))
    //                {
    //                    return $"@{whereClause.ParameterName}";
    //                }

    //                return $"@{SetParameterName(whereClause)}";

    //            default:
    //                throw new NotFound(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
    //        }
    //    }

    //    return null;
    //}


    //private static WhereClause? ReduceWhereClause(WhereClause? parameter)
    //{
    //    if (parameter == null) return null;
    //    switch (parameter.PartType)
    //    {
    //        case PartType.ColumnInfo when parameter.ColumnPropertyInfo != null:
    //            {
    //                if (parameter.ClauseType != ClauseType.Having)
    //                    return parameter;

    //                return null;
    //            }
    //        case PartType.ParameterInfo:
    //            return parameter;
    //        case PartType.WhereClause:
    //            var leftParameter = ReduceWhereClause(parameter.Left);
    //            var rightParameter = ReduceWhereClause(parameter.Right);

    //            if (leftParameter != null && rightParameter == null && leftParameter.PartType != PartType.ParameterInfo)
    //                return new WhereClause(leftParameter, null, ConditionOperatorType.None, parameter.ClauseType);

    //            if (rightParameter != null && leftParameter == null && rightParameter.PartType != PartType.ParameterInfo)
    //                return new WhereClause(rightParameter, null, ConditionOperatorType.None, parameter.ClauseType);

    //            if (leftParameter != null && rightParameter != null)
    //                return new WhereClause(leftParameter, rightParameter, parameter.Operator, parameter.ClauseType);
    //            return null;
    //    }

    //    return null;
    //}

    //private static string GenerateClausePhrase(string left, ConditionOperatorType operatorType, string? right, bool isString, bool isArray = false)
    //{
    //    var startQuote = isString ? "N'" : string.Empty;
    //    var endQuote = isString ? "'" : string.Empty;
    //    return operatorType switch
    //    {
    //        ConditionOperatorType.Equal => $"{left} = {right}",
    //        ConditionOperatorType.NotEqual => $"{left} <> {right}",
    //        ConditionOperatorType.Like => $"{left} LIKE {right}",
    //        ConditionOperatorType.NotLike => $"{left} NOT LIKE {right}",
    //        ConditionOperatorType.LeftLike => $"{left} LIKE %{right}",
    //        ConditionOperatorType.NotLeftLike => $"{left} NOT LIKE %{right}{endQuote}",
    //        ConditionOperatorType.RightLike => $"{left} LIKE {right}%",
    //        ConditionOperatorType.NotRightLike => $"{left} NOT LIKE {right}%",
    //        ConditionOperatorType.GreaterThan => $"{left} > {right}",
    //        ConditionOperatorType.GreaterThanEqual => $"{left} >= {right}",
    //        ConditionOperatorType.LessThan => $"{left} < {right}",
    //        ConditionOperatorType.LessThanEqual => $"{left} <= {right}",
    //        ConditionOperatorType.In => $"{left} IN ({right})",
    //        ConditionOperatorType.Contains => isArray ? $"{left} IN ({right})" : isString ? $"{left} LIKE {right}" : throw new NotSupported(left, ExceptionCode.CacheQueryFilteringGenerator),
    //        ConditionOperatorType.NotIn => $"{left} NOT IN ({right})",
    //        ConditionOperatorType.IsNull => $"{left} IS NULL",
    //        ConditionOperatorType.IsNotNull => $"{left} IS NOT NULL",
    //        ConditionOperatorType.Or => right == null ? $"({right})" : $"({left} OR {right})",
    //        ConditionOperatorType.And => right == null ? $"({right})" : $"({left} AND {right})",
    //        ConditionOperatorType.None => $"({left})",
    //        //I think we should define parameter name in this item
    //        ConditionOperatorType.Set => $"",
    //        ConditionOperatorType.Between or _ => throw new NotSupported(left, ExceptionCode.CacheQueryFilteringGenerator)
    //    };
    //}

    ////ToDo : add these methods in helper for use by all QueryPart
    //private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    //private static string SetParameterName(WhereClause item) => item.ParameterName + (item.Index > 0 ? $"_{item.Index}" : "");

    //public void ReduceParameter(WhereClause? parameter = null, Dictionary<string, int>? parameterIndex = null)
    //{
    //    parameterIndex ??= new Dictionary<string, int>();
    //    parameter ??= Parameter;

    //    switch (parameter.Left)
    //    {
    //        case { PartType: PartType.ParameterInfo } when parameter?.Left.ParameterName is null:
    //            throw new ArgumentNullException($"Parameter can not be null for {parameter?.ColumnPropertyInfo?.ColumnName}.");
    //        case { PartType: PartType.ParameterInfo }:
    //            {
    //                SetParameterIndex(parameter.Left, parameterIndex);
    //                parameter?.Left.SetIndex(parameterIndex[parameter?.Left.ParameterName!]);
    //                break;
    //            }
    //        case { PartType: PartType.WhereClause }:
    //            ReduceParameter(parameter.Left);
    //            break;
    //    }

    //    if (parameter?.Right is { ParameterName: { } })
    //    {
    //        switch (parameter?.Right)
    //        {
    //            case { PartType: PartType.ParameterInfo } when parameter?.Right.ParameterName is null:
    //                throw new ArgumentNullException($"Parameter can not be null for {parameter?.ColumnPropertyInfo?.ColumnName}.");
    //            case { PartType: PartType.ParameterInfo }:
    //                {
    //                    SetParameterIndex(parameter.Right, parameterIndex);
    //                    parameter?.Right.SetIndex(parameterIndex[parameter?.Right.ParameterName!]);
    //                    break;
    //                }
    //            case { PartType: PartType.WhereClause }:
    //                ReduceParameter(parameter.Right);
    //                break;
    //        }
    //    }
    //}

    //private static void SetParameterIndex(WhereClause parameter, Dictionary<string, int> parameterIndex)
    //{
    //    if (parameterIndex.ContainsKey(parameter?.ParameterName!))
    //    {
    //        parameterIndex[parameter?.ParameterName!] = ++parameterIndex[parameter?.ParameterName!];
    //    }
    //    else
    //        parameterIndex.Add(parameter?.ParameterName!, 0);
    //}
}

public class CacheWhereClause
{
    public LambdaExpression Predicate
    {
        get;
    }

    public CacheWhereClause(LambdaExpression predicate)
    {
        Predicate = predicate;
    }
}
