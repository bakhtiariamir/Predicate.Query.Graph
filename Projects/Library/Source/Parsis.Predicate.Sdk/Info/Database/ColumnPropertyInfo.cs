using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;

public class ColumnPropertyInfo : PropertyInfo<IColumnPropertyInfo>, IColumnPropertyInfo
{
    public string DataSet
    {
        get;
        private set;
    }

    public string Schema
    {
        get;
        private set;
    }

    public string ColumnName
    {
        get;
        private set;
    }

    public bool Identity
    {
        get;
    }

    public DatabaseFieldType FieldType
    {
        get;
    }

    public AggregateFunctionType? AggregateFunctionType
    {
        get;
    }

    public RankingFunctionType? RankingFunctionType
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

    public string? FunctionName
    {
        get;
    }

    public IColumnPropertyInfo? Parent
    {
        get;
        set;
    }

    public ColumnPropertyInfo() : base()
    {
    }

    public ColumnPropertyInfo(string schema, string dataSet, string columnName, string name, bool key, bool identity, ColumnDataType dataType, DatabaseFieldType fieldType, Type type, bool isUnique = false, bool readOnly = false, bool notMapped = false, string? functionName = null, AggregateFunctionType? aggregateFunctionType = null, RankingFunctionType? rankingFunctionType = null, bool required = false, string? title = null, IDictionary<string, string>? errorMessage = null, string[]? windowPartitionColumns = null, string[]? windowOrderColumns = null, object? defaultValue = null, bool isObject = false, int? maxLength = null, int? minLength = null, string? uniqueFieldGroup = null) : base(key, name, isUnique, readOnly, notMapped, dataType, type, required, title, errorMessage, defaultValue, isObject, maxLength, minLength, uniqueFieldGroup)
    {
        Schema = schema;
        DataSet = dataSet;
        ColumnName = columnName;
        Key = key;
        FieldType = fieldType;
        Identity = identity;
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
        if (Parent is not null)
            return $"[{Parent.Name}]";

        return $"[{Schema}].[{DataSet}]";
    }

    public string GetJoinSelector() => $"[{Name}]";

    public string GetJoinCreateSelector() => $"[{Schema}].[{DataSet}] AS {Name}";

    public string GetCombinedAlias(bool getParent = false)
    {
        if (Parent is not null)
        {
            if (Parent.Parent != null)
            {
                if (getParent)
                    return $"{Parent.GetCombinedAlias(true)}_{Name}";

                return $"{Parent.GetCombinedAlias(true)}_{ColumnName}";
            }
            
            return $"{Parent.Name}_{Name}";
        }

        if (!getParent) return $"{DataSet}_{Name}";

        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return true;

        var column = (IColumnPropertyInfo)obj;

        if (column.DataSet == DataSet && column.Schema == Schema && column.ColumnName == ColumnName && column.Name == Name)
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

    public override IColumnPropertyInfo Clone() => new ColumnPropertyInfo(Schema, DataSet, ColumnName, Name, Key, Identity, DataType, FieldType, Type, Identity, IsUnique, NotMapped, FunctionName, AggregateFunctionType, RankingFunctionType, Required, Title, ErrorMessage, WindowPartitionColumns, WindowOrderColumns, UniqueFieldGroup);
}
