using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public abstract class PropertyInfo<TProperty> : IPropertyInfo<TProperty> where TProperty : IPropertyInfo
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

    public string? ErrorMessage
    {
        get;
        set;
    }

    public object? DefaultValue
    {
        get;
        set;
    }

    protected PropertyInfo()
    {
    }

    protected PropertyInfo(string name, bool isUnique, ColumnDataType dataType, Type type, bool required = false, string? title = null, string? @alias = null, string? errorMessage = null, object? defaultValue = null)
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
    }

    public abstract TProperty Clone();

    public Type Type
    {
        get;
    }
}
