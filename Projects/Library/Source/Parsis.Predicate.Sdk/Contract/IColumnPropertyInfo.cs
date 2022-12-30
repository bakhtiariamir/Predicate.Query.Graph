﻿using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IColumnPropertyInfo : IPropertyInfo<IColumnPropertyInfo>
{
    bool IsPrimaryKey
    {
        get;
    }

    bool IsIdentity
    {
        get;
    }

    bool ReadOnly
    {
        get;
    }

    bool NotMapped
    {
        get;
    }

    string DataSet
    {
        get;
    }

    string Schema
    {
        get;
    }

    string ColumnName
    {
        get;
    }
    DatabaseFieldType FieldType
    {
        get;
    }

    AggregateFunctionType? AggregateFunctionType
    {
        get;
    }

    RankingFunctionType? RankingFunctionType
    {
        get;
    }

    string[]? WindowPartitionColumns
    {
        get;
    }

    string[]? WindowOrderColumns
    {
        get;
    }

    string? FunctionName
    {
        get;
    }

    IColumnPropertyInfo? Parent
    {
        get;
        set;
    }

    void SetRelationalObject(IColumnPropertyInfo propertyInfo);
    string GetSelector();
    string GetCombinedAlias();
    void SetParameterData(string schema, string dataSet, string name, string columnName, ColumnDataType objectType);
    void SetSchemaDataSet(string schema, string dataSet);
}