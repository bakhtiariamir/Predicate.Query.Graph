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

    public bool IsIdentity
    {
        get;
    }

    public bool ReadOnly
    {
        get;
    }

    public bool NotMapped
    {
        get;
    }

    public DatabaseFieldType Type
    {
        get;
    }

    public AggregateFunctionType AggregateFunctionType
    {
        get;
    }

    public RankingFunctionType RankingFunctionType
    {
        get;
    }

    public string[]? WindowPartitionColumns
    {
        get;
    }

    public string[]? WindowOrderColumns
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

    public ColumnInfoAttribute(string columnName, ColumnDataType dataType, DatabaseFieldType type, string? name = null, bool isUnique = false, bool isPrimaryKey = false, bool isIdentity = false, bool required = false, bool readOnly = false, bool notMapped = false, AggregateFunctionType aggregateFunctionType = AggregateFunctionType.None, RankingFunctionType rankingFunctionType = RankingFunctionType.None, string[]? windowPartitionColumns = null, string[]? windowOrderColumns = null, string functionName = "", string? title = null, string? errorMessage = null, object? defaultValue = null) : base(name ?? columnName, isUnique, errorMessage, required, defaultValue)
    {
        ColumnName = columnName;
        IsPrimaryKey = isPrimaryKey;
        DataType = dataType;
        Type = type;
        IsIdentity = isIdentity;
        ReadOnly = readOnly;
        NotMapped = notMapped;
        WindowOrderColumns = windowOrderColumns;
        WindowPartitionColumns = windowPartitionColumns;
        RankingFunctionType = rankingFunctionType;
        AggregateFunctionType = aggregateFunctionType;
        FunctionName = functionName;
        Title = title ?? Name;
    }
}
