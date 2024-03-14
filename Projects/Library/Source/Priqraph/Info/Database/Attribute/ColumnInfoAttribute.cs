using Priqraph.DataType;

namespace Priqraph.Info.Database.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnInfoAttribute : BasePropertyAttribute
{
    public string ColumnName
    {
        get;
    }

    public bool Identity
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

    public ColumnInfoAttribute(string columnName, ColumnDataType dataType, DatabaseFieldType type, string? name = null, bool isUnique = false, bool key = false, bool identity = false, bool required = false, bool readOnly = false, bool notMapped = false, AggregateFunctionType aggregateFunctionType = AggregateFunctionType.None, RankingFunctionType rankingFunctionType = RankingFunctionType.None, string[]? windowPartitionColumns = null, string[]? windowOrderColumns = null, string functionName = "", string? title = null, object? defaultValue = null, int maxLength = 0, int minLength = 0, string? uniqueFieldGroup = null, string? regexValidator = null, string? regexError = null) : base(key, name ?? columnName, isUnique, dataType, readOnly, notMapped, title, required, defaultValue, maxLength, minLength, uniqueFieldGroup, regexValidator, regexError)
    {
        ColumnName = columnName;
        Type = type;
        Identity = identity;
        WindowOrderColumns = windowOrderColumns;
        WindowPartitionColumns = windowPartitionColumns;
        RankingFunctionType = rankingFunctionType;
        AggregateFunctionType = aggregateFunctionType;
        FunctionName = functionName;
    }
}
