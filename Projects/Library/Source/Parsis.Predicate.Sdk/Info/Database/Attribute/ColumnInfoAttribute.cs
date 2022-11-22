using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database.Attribute;
[AttributeUsage(AttributeTargets.Property)]
public class ColumnInfoAttribute : BasePropertyAttribute
{

    public string ColumnName
    {
        get;
    }
    public bool IsPrimaryKey
    {
        get;
    }
    public DatabaseFieldType Type
    {
        get;
    }

    public AggregationFunctionType AggregationFunctionType
    {
        get;
    }

    public string FunctionName
    {
        get;
    }

    public ColumnDataType DataType
    {
        get;
    }

    public string Title
    {
        get;
    }


    public ColumnInfoAttribute(string columnName, ColumnDataType dataType, DatabaseFieldType type, string? name = null, bool isPrimaryKey = false, bool required = false, AggregationFunctionType aggregationFunctionType = AggregationFunctionType.None, string functionName = "", string? title = null, string? errorMessage = null) : base(name ?? columnName, errorMessage, required)
    {
        ColumnName = columnName;
        IsPrimaryKey = isPrimaryKey;
        DataType = dataType;
        Type = type;
        AggregationFunctionType = aggregationFunctionType;
        FunctionName = functionName;
        Title = title ?? Name;
    }

}
