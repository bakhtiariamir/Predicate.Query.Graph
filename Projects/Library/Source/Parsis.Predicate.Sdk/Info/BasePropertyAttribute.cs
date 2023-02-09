using Parsis.Predicate.Sdk.DataType;

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

    public Dictionary<string, string>? ErrorMessage
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

    public ColumnDataType DataType
    {
        get;
    }

    public string Title
    {
        get;
    }

    protected BasePropertyAttribute(string name, bool isUnique, ColumnDataType dataType, string? title = null, bool? required = null, object? defaultValue = null, params string[]? errorMessage)
    {
        Name = name;
        IsUnique = isUnique;
        Required = required;
        DefaultValue = defaultValue;
        Title = title ?? Name;
        DataType = dataType;
        if (errorMessage != null)
        {
            ErrorMessage = new Dictionary<string, string>();
            for (var i = 0; i < errorMessage.Length; i += 2)
                ErrorMessage.Add(errorMessage[i], errorMessage[i + 1]);
        }
    }
}
