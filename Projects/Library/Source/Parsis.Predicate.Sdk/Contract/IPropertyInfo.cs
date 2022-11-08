using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IPropertyInfo
{
    string Title
    {
        get;
    }

    string ErrorMessage
    {
        get;
    }

    PropertyDataType Type
    {
        get;
    }
}