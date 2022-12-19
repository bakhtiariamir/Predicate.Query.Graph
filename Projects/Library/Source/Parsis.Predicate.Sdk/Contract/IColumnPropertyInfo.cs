using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IColumnPropertyInfo : IPropertyInfo<IColumnPropertyInfo>
{
    bool IsPrimaryKey
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

    AggregationFunctionType? AggregationFunctionType
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

    string? ParameterName
    {
        get;
    }

    void SetRelationalObject(IColumnPropertyInfo propertyInfo);
    string GetSelector();
    string GetCombinedAlias();
    void SetParameterData(string schema, string dataSet, string name, string columnName, ColumnDataType objectType);
    void SetSchemaDataSet(string schema, string dataSet);
}