using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabaseColumnsClauseQueryPart : DatabaseQueryPart<ICollection<IColumnPropertyInfo>>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private DatabaseColumnsClauseQueryPart(ICollection<IColumnPropertyInfo> properties) => Parameter = properties;

    private string SetColumnName(IColumnPropertyInfo item) => item.AggregateFunctionType switch {
        AggregateFunctionType.Count => $"COUNT(*){SetOverPartition(item)} AS COUNT_{item.GetCombinedAlias()}",
        AggregateFunctionType.Average => $"AVG({SetColumnSelector(item)}) {SetOverPartition(item)} AS AVG_{item.GetCombinedAlias()}",
        AggregateFunctionType.Max => $"MAX({SetColumnSelector(item)}) {SetOverPartition(item)} AS MAX_{item.GetCombinedAlias()}",
        AggregateFunctionType.Min => $"MIN({SetColumnSelector(item)}) {SetOverPartition(item)} AS MIN_{item.GetCombinedAlias()}",
        AggregateFunctionType.Sum => $"SUM({SetColumnSelector(item)}) {SetOverPartition(item)} AS SUM_{item.GetCombinedAlias()}",
        AggregateFunctionType.None or _ or null => $"{SetColumnSelector(item)} As {item.Alias ?? item.GetCombinedAlias()}"
        //اگر ستون جوین بود شناسه رو نزاره مثل Domain.parentId  چون تو خودش داره یا هر روش دیگه ای
        //
    };

    private string? SetOverPartition(IColumnPropertyInfo columnPropertyInfo)
    {
        switch (columnPropertyInfo)
        {
            case {WindowPartitionColumns: {Length : > 0}}:
                {
                    var partitionKeys = columnPropertyInfo.WindowPartitionColumns.Select(partitionColumn => SetColumnSelector(Parameter.FirstOrDefault(item => item.ColumnName == partitionColumn) ?? throw new NotFound(partitionColumn, ExceptionCode.DatabaseQueryGroupByGenerator)));
                    return $"OVER({string.Join(", ", partitionKeys)})";
                }
            default:
                return null;
        }
    }

    private static string SetColumnSelector(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private void SetText() => _text = string.Join(", ", Parameter.Select(SetColumnName));

    public static DatabaseColumnsClauseQueryPart Create(params IColumnPropertyInfo[] properties) => new(properties);

    public static DatabaseColumnsClauseQueryPart Merged(IEnumerable<DatabaseColumnsClauseQueryPart> columnsClause)
    {
        var list = columnsClause.SelectMany(properties => properties.Parameter).DistinctBy(item => new {
            item.Schema,
            item.DataSet,
            item.Name,
            item.ColumnName,
            item.Parent
        }).ToList();

        var columns = list.Where(property => list.All(item => item.DataSet != property.Name || (item.Parent != null && item.IsPrimaryKey))).ToList();

        var column = new DatabaseColumnsClauseQueryPart(columns);
        column.SetText();

        return column;
    }
}
