using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public class DatabaseColumnsClauseQueryPart : DatabaseQueryPart<ICollection<IColumnPropertyInfo>>
{
    private string? _text;
    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    protected override QueryPartType QueryPartType => QueryPartType.Columns;

    private DatabaseColumnsClauseQueryPart(ICollection<IColumnPropertyInfo> properties) => Parameter = properties;

    private string SetColumnName(IColumnPropertyInfo item)
    {
        return item.AggregationFunctionType switch
        {
            AggregationFunctionType.Count => $"COUNT(*) AS COUNT",
            AggregationFunctionType.Average => $"AVG({SetColumnSelector(item)}) AS AVG_{item.GetCombinedAlias()}",
            AggregationFunctionType.Max => $"MAX({SetColumnSelector(item)}) AS MAX_{item.GetCombinedAlias()}",
            AggregationFunctionType.Min => $"MIN({SetColumnSelector(item)}) AS MIN_{item.GetCombinedAlias()}",
            AggregationFunctionType.Sum => $"SUM({SetColumnSelector(item)}) AS SUM_{item.GetCombinedAlias()}",
            AggregationFunctionType.None or _ or null => $"{SetColumnSelector(item)} As {item.Alias ?? item.GetCombinedAlias()}"
        };
    }

    private string SetColumnSelector(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private void SetText()
    {
        _text = string.Join(", ", Parameter.Select(SetColumnName));
    }

    public static DatabaseColumnsClauseQueryPart Create(params IColumnPropertyInfo[] properties) => new(properties);

    public static DatabaseColumnsClauseQueryPart CreateMerged(IEnumerable<DatabaseColumnsClauseQueryPart> columnsClause)
    {
        var columns = new List<IColumnPropertyInfo>();
        var list = columnsClause.SelectMany(properties => properties.Parameter).DistinctBy(item => new
        {
            item.Schema,
            item.DataSet,
            item.Name,
            item.ColumnName
        }).ToList();

        foreach (var property in list)
        {
            if (list.All(item => item.DataSet != property.Name))
                columns.Add(property);
        }

        var column = new DatabaseColumnsClauseQueryPart(columns);
        column.SetText();

        return column;
    }

}
