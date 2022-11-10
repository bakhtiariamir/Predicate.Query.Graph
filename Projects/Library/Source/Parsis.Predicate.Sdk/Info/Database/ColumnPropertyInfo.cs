using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;

public class ColumnPropertyInfo : PropertyInfo, IColumnPropertyInfo
{
    public DatabaseFieldType FieldType
    {
        get;
    }

    public AggregationFunctionType? AggregationFunctionType
    {
        get;
    }

    public string? FunctionName
    {
        get;
    }

    public string? FkAlias
    {
        get;
    }

    public RelationType? RelationType
    {
        get;
    }

    public ColumnPropertyInfo(string name, PropertyDataType dataType, DatabaseFieldType fieldType, string? functionName = null, string? fkAlias = null, AggregationFunctionType? aggregationFunctionType = null, RelationType? relationType = null, bool? required = null, string? title = null, string? alias = null, string? errorMessage = null) : base(name, dataType, required, title, alias, errorMessage)
    {
        FieldType = fieldType;
        AggregationFunctionType = aggregationFunctionType;
        FunctionName = functionName;
        FkAlias = fkAlias;
        RelationType = relationType;
    }

}