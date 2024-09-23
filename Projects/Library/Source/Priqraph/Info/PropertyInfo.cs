using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Info;

public class PropertyInfo<TProperty> : PropertyInfo, IPropertyInfo<TProperty> where TProperty : IPropertyInfo
{
    protected PropertyInfo()
    {
    }

    protected PropertyInfo(bool key, string name, bool isUnique, bool readOnly, bool notMapped, ColumnDataType dataType, Type type, bool required = false, string? title = null, object? defaultValue = null, bool isObject = false, int? maxLength = null, int? minLength = null, string? uniqueFieldGroup = null, string? regexValidator = null, string? regexError = null) : base(key, name, isUnique, readOnly, notMapped, dataType, type, required, title, defaultValue, isObject, maxLength, minLength, uniqueFieldGroup, regexValidator, regexError)
    {
    }

    public virtual TProperty Clone() => throw new NotImplementedException();
}

public class PropertyInfo : IPropertyInfo
{
    public bool Key
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    } = string.Empty;

    public bool IsUnique
    {
        get;
        set;
    }

    public string? UniqueFieldGroup
    {
        get;
        set;
    }

    public bool ReadOnly
    {
        get;
        set;
    }

    public bool NotMapped
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    } = string.Empty;

    public ColumnDataType DataType
    {
        get;
        set;
    }

    public bool Required
    {
        get;
        set;
    }

    public int? MaxLength
    {
        get;
        set;
    }

    public int? MinLength
    {
        get;
        set;
    }

    public object? DefaultValue
    {
        get;
        set;
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

    public Type Type
    {
        get;
    } = typeof(object);

    public bool IsObject
    {
        get;
        set;
    }

    public PropertyInfo()
    {
    }

    public PropertyInfo(bool key, string name, bool isUnique, bool readOnly, bool notMapped, ColumnDataType dataType, Type type, bool required = false, string? title = null, object? defaultValue = null, bool isObject = false, int? maxLength = null, int? minLength = null, string? uniqueFieldGroup = null, string? regexValidator = null, string? regexError = null)
    {
        Key = key;
        Name = name;
        IsUnique = isUnique;
        ReadOnly = readOnly;
        NotMapped = notMapped;
        Title = title ?? Name;
        DataType = dataType;
        Required = required;
        DefaultValue = defaultValue;
        Type = type;
        IsObject = isObject;
        MaxLength = maxLength;
        MinLength = minLength;
        UniqueFieldGroup = uniqueFieldGroup;
        RegexValidator = regexValidator;
        RegexError = regexError;
    }

    public IPropertyInfo ClonePropertyInfo() => new PropertyInfo(Key, Name, IsUnique, ReadOnly, NotMapped, DataType, Type, Required, Title, DefaultValue, IsObject, MaxLength, MinLength, UniqueFieldGroup, RegexValidator, RegexError);
}
