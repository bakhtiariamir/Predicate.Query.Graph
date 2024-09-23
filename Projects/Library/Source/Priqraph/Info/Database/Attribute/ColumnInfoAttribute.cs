using Priqraph.DataType;

namespace Priqraph.Info.Database.Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnInfoAttribute(
    string columnName,
    ColumnDataType dataType,
    DatabaseFieldType type,
    string? name = null,
    bool isUnique = false,
    bool key = false,
    bool identity = false,
    bool required = false,
    bool readOnly = false,
    bool notMapped = false,
    AggregateFunctionType aggregateFunctionType = AggregateFunctionType.None,
    RankingFunctionType rankingFunctionType = RankingFunctionType.None,
    string[]? windowPartitionColumns = null,
    string[]? windowOrderColumns = null,
    string functionName = "",
    string? title = null,
    object? defaultValue = null,
    int maxLength = 0,
    int minLength = 0,
    string? uniqueFieldGroup = null,
    string? regexValidator = null,
    string? regexError = null)
    : BasePropertyAttribute(key, name ?? columnName, isUnique, dataType, readOnly, notMapped, title, required,
        defaultValue, maxLength, minLength, uniqueFieldGroup, regexValidator, regexError)
{
    public string ColumnName
    {
        get;
    } = columnName;

    public bool Identity
    {
        get;
    } = identity;

    public DatabaseFieldType Type
    {
        get;
    } = type;

    public AggregateFunctionType AggregateFunctionType
    {
        get;
    } = aggregateFunctionType;

    public RankingFunctionType RankingFunctionType
    {
        get;
    } = rankingFunctionType;

    public string[]? WindowPartitionColumns
    {
        get;
    } = windowPartitionColumns;

    public string[]? WindowOrderColumns
    {
        get;
    } = windowOrderColumns;

    public string FunctionName
    {
        get;
    } = functionName;
}
