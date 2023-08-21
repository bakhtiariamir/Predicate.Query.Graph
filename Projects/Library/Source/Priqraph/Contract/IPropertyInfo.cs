﻿using Priqraph.DataType;

namespace Priqraph.Contract;

public interface IPropertyInfo<out TProperty> : IPropertyInfo where TProperty : IPropertyInfo
{
    TProperty Clone();
}

public interface IPropertyInfo
{
    bool Key
    {
        get;
    }

    string Name
    {
        get;
    }

    string? Title
    {
        get;
    }

    ColumnDataType DataType
    {
        get;
    }

    bool Required
    {
        get;
    }

    bool IsUnique
    {
        get;
    }
    
    string? UniqueFieldGroup
    {
        get;
        set;
    }
    
    bool ReadOnly
    {
        get;
    }
    
    bool NotMapped
    {
        get;
    }

    int? MaxLength
    {
        get;
    }

    int? MinLength
    {
        get;
    }

    object? DefaultValue
    {
        get;
    }

    IDictionary<string, string>? ErrorMessage
    {
        get;
    }

    Type Type
    {
        get;
    }

    bool IsObject
    {
        get;
    }

    IPropertyInfo ClonePropertyInfo();
}