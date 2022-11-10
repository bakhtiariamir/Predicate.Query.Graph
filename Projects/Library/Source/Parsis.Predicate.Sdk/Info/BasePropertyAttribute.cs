using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;
public class BasePropertyAttribute : Attribute
{
    public string Name
    {
        get;
    }

    public bool? Required
    {
        get;
    }
    public string? ErrorMessage
    {
        get;
    }

    public BasePropertyAttribute(string name, string? errorMessage = null, bool? required = null)
    {
        Name = name;
        ErrorMessage = errorMessage;
        Required = required;
    }
}
