using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public class ColumnPropertyInfo : PropertyInfo, IColumnPropertyInfo
{
    public bool IsNullable
    {
        get;
    }

    public string ColumnName
    {
        get;
    }

    public string Alias
    {
        get;
    }

    public ColumnPropertyInfo(string columnName, string @alias, string title, string errorMessage, PropertyDataType type, bool isNullable) : base(title, errorMessage, type)
    {
        IsNullable = isNullable;
        ColumnName = columnName;
        Alias = alias;
    }
}