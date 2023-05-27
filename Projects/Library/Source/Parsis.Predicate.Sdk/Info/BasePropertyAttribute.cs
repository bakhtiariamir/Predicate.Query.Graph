using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public abstract class BasePropertyAttribute : Attribute
{
    public bool Key
    {
        get;
    }

    public string Name
    {
        get;
    }

    public string Title
    {
        get;
    }

    public bool Required
    {
        get;
    }

    public bool ReadOnly
    {
        get;
    }

    public bool NotMapped
    {
        get;
    }

    public bool IsUnique
    {
        get;
    }
    
    public string? UniqueFieldGroup
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

    public ColumnDataType DataType
    {
        get;
    }

    public int MaxLength
    {
        get;
        set;
    }

    public int MinLength
    {
        get;
        set;
    }

    protected BasePropertyAttribute(bool key, string name, bool isUnique, ColumnDataType dataType, bool readOnly = false, bool notMapped = false, string? title = null, bool required = false, object? defaultValue = null, int maxLength = 0, int minLength = 0, string? uniqueFieldGroup = null, params string[]? errorMessage)
    {
        Key = key;
        Name = name;
        IsUnique = isUnique;
        Required = required;
        ReadOnly = readOnly;
        NotMapped = notMapped;
        DefaultValue = defaultValue;
        Title = title ?? Name;
        DataType = dataType;
        UniqueFieldGroup = uniqueFieldGroup;
        MaxLength = maxLength;
        MinLength = minLength;
        if (errorMessage == null) return;
        
        ErrorMessage = new Dictionary<string, string>();
        for (var i = 0; i < errorMessage.Length; i += 2)
            ErrorMessage.Add(errorMessage[i], errorMessage[i + 1]);
    }
}
