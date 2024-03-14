using Priqraph.DataType;

namespace Priqraph.Info;

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
    }

    public int MinLength
    {
        get;
    }

    public string? RegexValidator
    {
        get;
        set;
    }

    public string? RegexError
    {
        get;
        set;
    }

    protected BasePropertyAttribute(bool key, string name, bool isUnique, ColumnDataType dataType, bool readOnly = false, bool notMapped = false, string? title = null, bool required = false, object? defaultValue = null, int maxLength = 0, int minLength = 0, string? uniqueFieldGroup = null, string? regexValidator = null, string? regexError = null, params string[]? errorMessage)
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
        RegexValidator = regexValidator;
        RegexError = regexError;
    }
}
