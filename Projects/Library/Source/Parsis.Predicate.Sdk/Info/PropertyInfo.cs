using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public abstract class PropertyInfo : IPropertyInfo
{
    public string Title
    {
        get;
    }

    public string ErrorMessage
    {
        get;
    }

    public PropertyDataType Type
    {
        get;
    }

    protected PropertyInfo(string title, string errorMessage, PropertyDataType type)
    {
        Title = title;
        ErrorMessage = errorMessage;
        Type = type;
    }
}