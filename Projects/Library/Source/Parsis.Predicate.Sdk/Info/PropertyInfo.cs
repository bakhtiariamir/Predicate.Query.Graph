using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public abstract class PropertyInfo : IPropertyInfo
{
    public string Name
    {
        get;
    }
    public string Title
    {
        get;
    }


    public PropertyDataType DataType
    {
        get;
    }

    public bool? Required
    {
        get;
    }

    public string Alias
    {
        get;
    }

    public string? ErrorMessage
    {
        get;
    }

    protected PropertyInfo(string name, PropertyDataType dataType, bool? required = null, string? title = null, string? @alias = null, string? errorMessage = null)
    {
        Name = name;
        Title = title ?? Name;
        DataType = dataType;
        Required = required;
        Alias = alias ?? Name;
        ErrorMessage = errorMessage;
    }
}