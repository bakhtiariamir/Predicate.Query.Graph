using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
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
    protected override QueryPartType QueryPartType => QueryPartType.Where;

    private DatabaseWhereClauseQueryPart(WhereClause parameter)
    {
        Parameter = parameter;
    }

    public static DatabaseWhereClauseQueryPart Create(WhereClause property) => new(property);

    public void SetText() => _text = SetWhereClauseText(Parameter);

    public static ICollection<IColumnPropertyInfo> GetColumnProperties(WhereClause parameter)
    {
        var columnProperties = new List<IColumnPropertyInfo>();
        switch (parameter)
        {
            case { PartType: PartType.ColumnInfo }:
                columnProperties.Add(parameter.ColumnPropertyInfo);
                break;
            case { PartType: PartType.WhereClause }:
                columnProperties.AddRange((GetColumnProperties(parameter.Left)));
                columnProperties.AddRange((GetColumnProperties(parameter.Right)));
                break;
        }
        return columnProperties;
    }

    public static ICollection<WhereClause> GeHavingClause(WhereClause parameter)
    {
        var columnProperties = new List<WhereClause>();
        var left = parameter.Left ?? throw new System.Exception("asd");
        var right = parameter.Right ?? throw new System.Exception("asd");
 
        if (left.PartType == PartType.ColumnInfo && left.ColumnPropertyInfo != null)
        {
            if (left.ClauseType == ClauseType.Having)
            {
                left.SetValue(right.Value);
                left.SetOperator(parameter.Operator);

                columnProperties.Add(left);
            }
        }
        else
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
        else
            columnProperties.AddRange((GeHavingClause(right)));

        return columnProperties;
    }

    private bool CheckHavingClause(WhereClause parameter) => parameter.ClauseType == ClauseType.Having;

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
                    var wherePart = (WhereClause)whereClause;
                    if (wherePart.Left is not null)
                    {
                        var left = SetWhereClauseText(wherePart.Left);
                        if (wherePart.Right is null && wherePart.Operator != ConditionOperatorType.None)
                            throw new Parsis.Predicate.Sdk.Exception.NotFoundException(whereClause.ToString(), whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);

                        var right = SetWhereClauseText(wherePart.Right);
                        var isString = wherePart.Left.ColumnPropertyInfo != null && new[] {ColumnDataType.Char, ColumnDataType.String}.Contains(wherePart.Left.ColumnPropertyInfo.DataType);
                        return GenerateClausePhrase(left, wherePart.Operator, right, isString);
                    }
                    else
                        throw new Exception.NotFoundException(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
                case PartType.ParameterInfo:
                    if (whereClause.ColumnPropertyInfo == null) throw new NotFoundException(ExceptionCode.DatabaseQueryFilteringGenerator);
                    if (whereClause.Index == null) throw new NotFoundException(ExceptionCode.DatabaseQueryFilteringGenerator);

                    return SetParameterName(whereClause.ColumnPropertyInfo, whereClause.Index);
                default:
                    throw new Exception.NotFoundException(whereClause.ToString() ?? "Unknown", whereClause.PartType, ExceptionCode.DatabaseQueryFilteringGenerator);
            }
        }

        return null;
    }

    private static WhereClause? ReduceWhereClause(WhereClause parameter)
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
        return operatorType switch
        {
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
            ConditionOperatorType.IsNull => $"{left} IS NULL {right}",
            ConditionOperatorType.NotIsNull => $"{left} IS NOT NULL {right}",
            ConditionOperatorType.Or => right == null ? $"({right})" :  $"({left} OR {right})",
            ConditionOperatorType.And => right == null ? $"({right})" : $"({left} AND {right})",
            ConditionOperatorType.None => $"({left})",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new Exception.NotSupportedException(left, ExceptionCode.DatabaseQueryFilteringGenerator)
        };
    }

    //ToDo : add these methods in helper for use by all QueryPart
    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"@P_{item.GetCombinedAlias()}_{index ?? 0}";

    public  void ReduceParameter(WhereClause? parameter = null)
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
        else if (parameter.Right is { PartType: PartType.WhereClause })
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

    public WhereClause(WhereClause left, WhereClause? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where)
    {
        Left = left;
        Right = right;
        Operator = @operator;
        ClauseType = clauseType;
        PartType = PartType.WhereClause;
    }

    public WhereClause(IColumnPropertyInfo columnPropertyInfo, object? value = null, ConditionOperatorType condition = ConditionOperatorType.None, PartType partType = PartType.ColumnInfo, ClauseType clauseType = ClauseType.Where)
    {
        ColumnPropertyInfo = columnPropertyInfo;
        ClauseType = clauseType;
        Operator = condition;
        Value = value;
        PartType = partType;
    }

    public void SetOperator(ConditionOperatorType operatorType) => Operator = operatorType;

    public void SetValue(object value) => Value = value;

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
