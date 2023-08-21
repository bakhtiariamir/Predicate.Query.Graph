using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Info;

public class PropertyInfo<TProperty> : PropertyInfo, IPropertyInfo<TProperty> where TProperty : IPropertyInfo
{
    public PropertyInfo()
    {
    }

    public PropertyInfo(bool key, string name, bool isUnique, bool readOnly, bool notMapped, ColumnDataType dataType, Type type, bool required = false, string? title = null, IDictionary<string, string>? errorMessage = null, object? defaultValue = null, bool isObject = false, int? maxLength = null, int? minLength = null, string? uniqueFieldGroup = null) : base(key, name, isUnique, readOnly, notMapped, dataType, type, required, title, errorMessage, defaultValue, isObject, maxLength, minLength, uniqueFieldGroup)
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
    }

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
    }

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

    public IDictionary<string, string>? ErrorMessage
    {
        get;
        set;
    }

    public object? DefaultValue
    {
        get;
        set;
    }

    public Type Type
    {
        get;
    }

    public bool IsObject
    {
        get;
        set;
    }

    public PropertyInfo()
    {
    }

    public PropertyInfo(bool key, string name, bool isUnique, bool readOnly, bool notMapped, ColumnDataType dataType, Type type, bool required = false, string? title = null, IDictionary<string, string>? errorMessage = null, object? defaultValue = null, bool isObject = false, int? maxLength = null, int? minLength = null, string? uniqueFields = null)
    {
        Key = key;
        Name = name;
        IsUnique = isUnique;
        ReadOnly = readOnly;
        NotMapped = notMapped;
        Title = title ?? Name;
        DataType = dataType;
        Required = required;
        ErrorMessage = errorMessage;
        DefaultValue = defaultValue;
        Type = type;
        IsObject = isObject;
        MaxLength = maxLength;
        MinLength = minLength;
        UniqueFieldGroup = uniqueFields;
    }

    public IPropertyInfo ClonePropertyInfo() => new PropertyInfo(Key, Name, IsUnique, ReadOnly, NotMapped, DataType, Type, Required, Title, ErrorMessage, DefaultValue, IsObject, MaxLength, MinLength);
}
