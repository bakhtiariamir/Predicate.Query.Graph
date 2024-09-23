using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Generator.Database;


public class FilterProperty
{
    public FilterProperty? Left
    {
        get;
    }

    public FilterProperty? Right
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

    public FilterProperty(FilterProperty left, FilterProperty? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where, string? parameterName = null)
    {
        Left = left;
        Right = right;
        Operator = @operator;
        ParameterName = parameterName;
        ClauseType = clauseType;
        PartType = PartType.WhereClause;
    }

    public FilterProperty(IColumnPropertyInfo columnPropertyInfo, object? value = null, ConditionOperatorType condition = ConditionOperatorType.None, PartType partType = PartType.ColumnInfo, ClauseType clauseType = ClauseType.Where, string? parameterName = null, Type? valueType = null)
    {
        ColumnPropertyInfo = columnPropertyInfo;
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = clauseType;
        Operator = condition;
        Value = value;
        PartType = partType;
    }

    private FilterProperty(object? value = null, Type? valueType = null, string? parameterName = null)
    {
        ValueType = valueType;
        ParameterName = parameterName;
        ClauseType = ClauseType.None;
        Value = value;
        PartType = PartType.ParameterInfo;
    }

    public static FilterProperty CreateWhereClause(FilterProperty left, FilterProperty? right, ConditionOperatorType @operator, ClauseType clauseType = ClauseType.Where) => new(left, right, @operator, clauseType);

    public static FilterProperty CreateParameterClause(object? value, Type? valueType, string? parameterName) => new(value, valueType, parameterName);

    public void SetOperator(ConditionOperatorType operatorType) => Operator = operatorType;

    public void SetValue(object? value) => Value = value;

    public void SetIndex(int index) => Index = index;

    public void SetParameterColumnInfo(IColumnPropertyInfo columnPropertyInfo) => ColumnPropertyInfo = columnPropertyInfo;
}
