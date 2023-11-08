using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;

namespace Priqraph.Sql.Generator;
public class ColumnQueryFragment : DatabaseColumnQueryFragment 
{
    private ColumnQueryFragment(ICollection<IColumnPropertyInfo> properties)
    {
        Parameter = properties;
    }

    private string SetColumnName(IColumnPropertyInfo item) => item.AggregateFunctionType switch
    {
        AggregateFunctionType.Count => $"COUNT(*){SetOverPartition(item)} AS COUNT_{item.GetCombinedAlias()}",
        AggregateFunctionType.Average => $"AVG({SetColumnSelector(item)}) {SetOverPartition(item)} AS AVG_{item.GetCombinedAlias()}",
        AggregateFunctionType.Max => $"MAX({SetColumnSelector(item)}) {SetOverPartition(item)} AS MAX_{item.GetCombinedAlias()}",
        AggregateFunctionType.Min => $"MIN({SetColumnSelector(item)}) {SetOverPartition(item)} AS MIN_{item.GetCombinedAlias()}",
        AggregateFunctionType.Sum => $"SUM({SetColumnSelector(item)}) {SetOverPartition(item)} AS SUM_{item.GetCombinedAlias()}",
        _ or null => $"{SetColumnSelector(item)} As {item.GetCombinedAlias()}"
    };

    private string? SetOverPartition(IColumnPropertyInfo columnPropertyInfo)
    {
        if (Parameter is { Count: <= 0 }) return default;

        switch (columnPropertyInfo)
        {
            case { WindowPartitionColumns: { Length: > 0 } strings }:
                {
                    var partitionKeys = strings.Select(partitionColumn => SetColumnSelector(Parameter!.FirstOrDefault(item => item.ColumnName == partitionColumn) ?? throw new NotFound(partitionColumn, ExceptionCode.DatabaseQueryGroupByGenerator)));
                    return $"OVER({string.Join(", ", partitionKeys)})";
                }
            default:
                return default;
        }
    }

    public static ColumnQueryFragment Create(params IColumnPropertyInfo[] properties) => new(properties);

    public static ColumnQueryFragment CreateCount()
    {
        var queryPart = new ColumnQueryFragment(new List<IColumnPropertyInfo>())
        {
            Text = " COUNT(*) AS COUNT "
        };
        return queryPart;
    }

    public static ColumnQueryFragment Merged(IEnumerable<ColumnQueryFragment> columnsClause)
    {
        var list = columnsClause.SelectMany(properties => properties.Parameter ?? new List<IColumnPropertyInfo>()).DistinctBy(item => new
        {
            item.Schema,
            item.DataSet,
            item.Name,
            item.ColumnName,
            item.Parent
        }).ToList();
        var relatedObjects = list.Where(item => item.Type.GetInterface(nameof(IQueryableObject)) != null && item.Parent == null).ToArray();
        var removeRelatedObjects = new List<IColumnPropertyInfo>();
        if (relatedObjects.Any())
            removeRelatedObjects.AddRange(relatedObjects.Where(relatedObject => list.Any(item => item.Parent?.Name == relatedObject.Name)));

        var columns = list.Where(property => !(removeRelatedObjects.Contains(property) && property.Parent == null)).ToList();
        var column = new ColumnQueryFragment(columns);
        column.SetText();

        return column;
    }

    private static string SetColumnSelector(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private void SetText() => Text = Parameter is { Count: > 0 } ? string.Join(", ", Parameter!.Select(SetColumnName)) : string.Empty;
}
