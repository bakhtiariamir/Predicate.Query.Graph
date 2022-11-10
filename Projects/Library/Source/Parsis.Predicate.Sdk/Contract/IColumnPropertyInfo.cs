using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IColumnPropertyInfo : IPropertyInfo
{
    DatabaseFieldType FieldType
    {
        get;
    }

    AggregationFunctionType? AggregationFunctionType
    {
        get;
    }

    string? FunctionName
    {
        get;
    }

    string? FkAlias
    {
        get;
    }

    RelationType? RelationType
    {
        get;
    }
}