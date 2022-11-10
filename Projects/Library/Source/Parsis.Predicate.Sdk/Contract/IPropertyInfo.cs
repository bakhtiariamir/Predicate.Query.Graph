using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IPropertyInfo
{
    string Name
    {
        get;
    }

    PropertyDataType DataType
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
}