using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database.Attribute;
[AttributeUsage(AttributeTargets.Property)]
public class ColumnInfoAttribute : BasePropertyAttribute
{
    public bool IsPrimaryKey
    {
        get;
    }
    public DatabaseFieldType Type
    {
        get;
    }

    public AggregationFunctionType? AggregationFunctionType
    {
        get;
    }

    public string FunctionName
    {
        get;
    }

    public PropertyDataType DataType
    {
        get;
    }

    public string Alias
    {
        get;
    }

    public string Title
    {
        get;
    }


    public ColumnInfoAttribute(string name, bool isPrimaryKey, PropertyDataType dataType, DatabaseFieldType type, AggregationFunctionType? aggregationFunctionType = null, string functionName = "", bool? required = null, string? @alias = null, string? title = null, string? errorMessage = null) : base(name, errorMessage, required)
    {
        IsPrimaryKey = isPrimaryKey;
        DataType = dataType;
        Type = type;
        AggregationFunctionType = aggregationFunctionType;
        FunctionName = functionName;
        Alias = @alias ?? Name;
        Title = title ?? Name;
    }

}
