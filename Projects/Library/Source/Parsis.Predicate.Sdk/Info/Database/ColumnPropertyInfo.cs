using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;

public class ColumnPropertyInfo : PropertyInfo<IColumnPropertyInfo>, IColumnPropertyInfo
{
    public string DataSet
    {
        get;
        set;
    }

    public string Schema
    {
        get;
        set;
    }

    public string ColumnName
    {
        get;
        set;
    }

    public bool IsPrimaryKey
    {
        get;
        set;
    }
    public DatabaseFieldType FieldType
    {
        get;
        set;
    }

    public AggregationFunctionType? AggregationFunctionType
    {
        get;
        set;
    }

    public string? FunctionName
    {
        get;
        set;
    }

    public IColumnPropertyInfo? Parent
    {
        get;
        set;
    }

    public ColumnPropertyInfo()
    {

    }

    public ColumnPropertyInfo(string schema, string dataSet, string columnName, string name, bool isPrimaryKey, ColumnDataType dataType, DatabaseFieldType fieldType, string? functionName = null, AggregationFunctionType? aggregationFunctionType = null, RelationType? relationType = null, bool? required = null, string? title = null, string? alias = null, string? errorMessage = null) : base(name, dataType, required, title, alias, errorMessage)
    {
        Schema = schema;
        DataSet = dataSet;
        ColumnName = columnName;
        IsPrimaryKey = isPrimaryKey;
        FieldType = fieldType;
        AggregationFunctionType = aggregationFunctionType;
        FunctionName = functionName;
    }

    public override string ToString() => Name;


    public void SetRelationalObject(IColumnPropertyInfo propertyInfo) => Parent = propertyInfo;
    public string GetSelector() => $"[{Schema}].[{DataSet}]";

    public string GetCombinedAlias() => $"{Schema}{DataSet}_{Name}";

    public void SetSchemaDataSet(string schema, string dataSet)
    {
        this.Schema = schema;
        this.DataSet = dataSet;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return true;

        var column = (IColumnPropertyInfo)obj;

        if (column.DataSet == this.DataSet && column.Schema == this.Schema && column.ColumnName == this.ColumnName && column.Name == this.Name)
            return true;

        return false;
    }

    public override IColumnPropertyInfo Clone() =>
        new ColumnPropertyInfo
        {
            Name = this.Name,
            Title = this.Title,
            AggregationFunctionType = this.AggregationFunctionType,
            DataSet = this.DataSet,
            Alias = this.Alias,
            ColumnName = this.ColumnName,
            DataType = this.DataType,
            ErrorMessage = this.ErrorMessage,
            FieldType = this.FieldType,
            Parent = this.Parent,
            FunctionName = this.FunctionName,
            IsPrimaryKey = this.IsPrimaryKey,
            Schema = this.Schema,
            Required = this.Required
        };
}