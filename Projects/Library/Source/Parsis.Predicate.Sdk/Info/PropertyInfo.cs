using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public class PropertyInfo<TProperty> : PropertyInfo, IPropertyInfo<TProperty> where TProperty : IPropertyInfo
{
    public PropertyInfo()
    {
    }

    public PropertyInfo(string name, bool isUnique, ColumnDataType dataType, Type type, bool required = false, string? title = null, string? @alias = null, IDictionary<string, string>? errorMessage = null, object? defaultValue = null, bool isObject = false) : base(name, isUnique, dataType, type, required, title, alias, errorMessage, defaultValue, isObject)
    {
    }

    public virtual TProperty Clone()
    {
        throw new NotImplementedException();
    }
}

public class PropertyInfo : IPropertyInfo
{
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

    public string? Alias
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

    public PropertyInfo(string name, bool isUnique, ColumnDataType dataType, Type type, bool required = false, string? title = null, string? @alias = null, IDictionary<string, string>? errorMessage = null, object? defaultValue = null, bool isObject = false)

    {
        Name = name;
        IsUnique = isUnique;
        Title = title ?? Name;
        DataType = dataType;
        Required = required;
        Alias = alias;
        ErrorMessage = errorMessage;
        DefaultValue = defaultValue;
        Type = type;
        IsObject = isObject;
    }
}
