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

    public bool IsIdentity
    {
        get;
        set;
    }

    public bool ReadOnly
    {
        get;
        set;
    }

    public bool NotMapped
    {
        get;
        set;
    }

    public DatabaseFieldType FieldType
    {
        get;
        set;
    }

    public AggregateFunctionType? AggregateFunctionType
    {
        get;
        set;
    }

    public RankingFunctionType? RankingFunctionType
    {
        get;
        set;
    }

    public string[]? WindowPartitionColumns
    {
        get;
    }

    public string[]? WindowOrderColumns
    {
        get;
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

    public ColumnPropertyInfo() : base()
    {
    }

    public ColumnPropertyInfo(string schema, string dataSet, string columnName, string name, bool isPrimaryKey, bool isIdentity, ColumnDataType dataType, DatabaseFieldType fieldType, Type type, bool isUnique = false, bool readOnly = false, bool notMapped = false, string? functionName = null, AggregateFunctionType? aggregateFunctionType = null, RankingFunctionType? rankingFunctionType = null, bool required = false, string? title = null, string? alias = null, IDictionary<string, string>? errorMessage = null, string[]? windowPartitionColumns = null, string[]? windowOrderColumns = null, object? defaultValue = null) : base(name, isUnique, dataType, type, required, title, alias, errorMessage, defaultValue)
    {
        Schema = schema;
        DataSet = dataSet;
        ColumnName = columnName;
        IsPrimaryKey = isPrimaryKey;
        FieldType = fieldType;
        IsIdentity = isIdentity;
        ReadOnly = readOnly;
        NotMapped = notMapped;
        WindowPartitionColumns = windowPartitionColumns;
        WindowOrderColumns = windowOrderColumns;
        AggregateFunctionType = aggregateFunctionType;
        RankingFunctionType = rankingFunctionType;
        FunctionName = functionName;
    }

    public override string ToString() => Name;

    public void SetRelationalObject(IColumnPropertyInfo propertyInfo) => Parent = propertyInfo;

    public string GetSelector()
    {
        if (Parent is not null && Parent.Name != DataSet)
            return $"[{Parent.Name}]";

        return $"[{Schema}].[{DataSet}]";
    }

    public string GetJoinSelector() => $"[{Name}]";

    public string GetJoinCreateSelector() => $"[{Schema}].[{DataSet}] AS {Name}";

    public string GetCombinedAlias()
    {
        if (Parent is not null && Parent.Name != DataSet) return $"{Parent.Name}_{ColumnName}";

        return $"{DataSet}_{ColumnName}";
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

    public void SetParameterData(string schema, string dataSet, string name, string columnName, ColumnDataType objectType)
    {
        Schema = schema;
        DataSet = dataSet;
        ColumnName = columnName;
        Name = name;
    }

    public override IColumnPropertyInfo Clone() => new ColumnPropertyInfo(Schema, DataSet, ColumnName, Name, IsPrimaryKey, IsIdentity, DataType, FieldType, Type, IsIdentity, IsUnique, NotMapped, FunctionName, AggregateFunctionType, RankingFunctionType, Required, Title, Alias, ErrorMessage, WindowPartitionColumns, WindowOrderColumns);
}
