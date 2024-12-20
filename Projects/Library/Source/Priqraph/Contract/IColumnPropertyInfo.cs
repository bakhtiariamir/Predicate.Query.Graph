﻿using Priqraph.DataType;

namespace Priqraph.Contract;

public interface IColumnPropertyInfo : IPropertyInfo<IColumnPropertyInfo>
{
    bool Identity
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

    bool IsLabel
    {
        get;
        set;
    }

    void SetRelationalObject(IColumnPropertyInfo propertyInfo);

    string GetSelector();

    string GetJoinSelector();

    string GetJoinCreateSelector();

    string GetCombinedAlias(bool getParent = false);

    void SetParameterData(string schema, string dataSet, string name, string columnName, ColumnDataType objectType);
}
