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

    public bool? Required
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

    protected PropertyInfo()
    {

    }

    protected PropertyInfo(object value)
    {
    }

    protected PropertyInfo(string name, ColumnDataType dataType, bool? required = null, string? title = null, string? @alias = null, string? errorMessage = null)
    {
        Name = name;
        Title = title ?? Name;
        DataType = dataType;
        Required = required;
        Alias = alias;
        ErrorMessage = errorMessage;
    }

    public abstract TProperty Clone();
}