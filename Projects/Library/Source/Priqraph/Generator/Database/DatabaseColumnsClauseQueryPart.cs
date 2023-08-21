using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;

namespace Priqraph.Generator.Database;

public class DatabaseColumnsClauseQueryPart : DatabaseQueryPart<ICollection<IColumnPropertyInfo>>
{
    private string? _text;
    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private DatabaseColumnsClauseQueryPart(ICollection<IColumnPropertyInfo> properties)
    {
        Parameter = properties;
    }

    private string SetColumnName(IColumnPropertyInfo item) => item.AggregateFunctionType switch {
        AggregateFunctionType.Count => $"COUNT(*){SetOverPartition(item)} AS COUNT_{item.GetCombinedAlias()}",
        AggregateFunctionType.Average => $"AVG({SetColumnSelector(item)}) {SetOverPartition(item)} AS AVG_{item.GetCombinedAlias()}",
        AggregateFunctionType.Max => $"MAX({SetColumnSelector(item)}) {SetOverPartition(item)} AS MAX_{item.GetCombinedAlias()}",
        AggregateFunctionType.Min => $"MIN({SetColumnSelector(item)}) {SetOverPartition(item)} AS MIN_{item.GetCombinedAlias()}",
        AggregateFunctionType.Sum => $"SUM({SetColumnSelector(item)}) {SetOverPartition(item)} AS SUM_{item.GetCombinedAlias()}",
        _ or null => $"{SetColumnSelector(item)} As {item.GetCombinedAlias()}"
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

    public static DatabaseColumnsClauseQueryPart CreateCount()
    {
        var queryPart = new DatabaseColumnsClauseQueryPart(new List<IColumnPropertyInfo>());
        queryPart._text = " COUNT(*) AS COUNT ";
        return queryPart;
    }

    public static DatabaseColumnsClauseQueryPart Merged(IEnumerable<DatabaseColumnsClauseQueryPart> columnsClause)
    {
        var list = columnsClause.SelectMany(properties => properties.Parameter).DistinctBy(item => new {
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
        var column = new DatabaseColumnsClauseQueryPart(columns);
        column.SetText();

        return column;
    }
}
