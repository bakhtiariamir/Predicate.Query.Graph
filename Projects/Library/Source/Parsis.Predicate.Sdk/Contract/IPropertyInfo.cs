using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IPropertyInfo<out TProperty> : IPropertyInfo  where TProperty : IPropertyInfo
{
    string Name
    {
        get;
    }

    string? Title
    {
        get;
    }

    string? Alias
    {
        get;
    }

    ColumnDataType DataType
    {
        get;
    }

    bool? Required
    {
        get;
    }

    string? ErrorMessage
    {
        get;
    }

    TProperty Clone();
}

public interface IPropertyInfo
{

}