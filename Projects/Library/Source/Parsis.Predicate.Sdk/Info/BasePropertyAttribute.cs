namespace Parsis.Predicate.Sdk.Info;

public abstract class BasePropertyAttribute : Attribute
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

    public object? DefaultValue
    {
        get;
    }

    public bool IsUnique
    {
        get;
    }

    protected BasePropertyAttribute(string name, bool isUnique, string? errorMessage = null, bool? required = null, object? defaultValue = null)
    {
        Name = name;
        IsUnique = isUnique;
        ErrorMessage = errorMessage;
        Required = required;
        DefaultValue = defaultValue;
    }
}
