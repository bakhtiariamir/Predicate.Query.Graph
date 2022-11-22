using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public class DatabaseColumnsClauseQueryPart<TObject> : DatabaseQueryPart<TObject, IColumnPropertyInfo> where TObject : class
{
    private string? _text;
    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    protected override QueryPartType QueryPartType => QueryPartType.Columns;

    public DatabaseColumnsClauseQueryPart() { }

    DatabaseColumnsClauseQueryPart(string? text) => _text = text;

    DatabaseColumnsClauseQueryPart(string? text, IColumnPropertyInfo property)
    {
        _text = text;
        Parameters = new[] { property };
    }

    DatabaseColumnsClauseQueryPart(IColumnPropertyInfo property) => Parameters = new[] { property };

    DatabaseColumnsClauseQueryPart(IList<IColumnPropertyInfo> properties) =>  Parameters = properties;

    private string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}] As {item.Alias ?? item.GetCombinedAlias()}";

    private void SetText()
    {
        _text = string.Join(", ", Parameters.Select(SetColumnName));
    }

    DatabaseColumnsClauseQueryPart(string? text, IList<IColumnPropertyInfo> properties)
    {
        _text = text;
        Parameters = properties;
    }

    public static DatabaseColumnsClauseQueryPart<TObject> Create(string? text) => new(text);
    public static DatabaseColumnsClauseQueryPart<TObject> Create(params IColumnPropertyInfo[] properties) => new(properties);

    public static DatabaseColumnsClauseQueryPart<TObject> Create(string? text, params IColumnPropertyInfo[] properties) => new(text, properties);
    public static DatabaseColumnsClauseQueryPart<TObject> CreateMerged(string? text, params DatabaseColumnsClauseQueryPart<TObject>[] sqlClauses) => new(text, sqlClauses.SelectMany(properties => properties.Parameters).ToList());
    public static DatabaseColumnsClauseQueryPart<TObject> CreateMerged(string? text, IEnumerable<DatabaseColumnsClauseQueryPart<TObject>> sqlQueries) => new(text, sqlQueries.SelectMany(properties => properties.Parameters).ToList());

    public static DatabaseColumnsClauseQueryPart<TObject> CreateMerged(IEnumerable<DatabaseColumnsClauseQueryPart<TObject>> sqlQueries)
    {
        var columns = new List<IColumnPropertyInfo>();
        var list = sqlQueries.SelectMany(properties => properties.Parameters).DistinctBy(item => new
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

        var column = new DatabaseColumnsClauseQueryPart<TObject>(columns);
        column.SetText();

        return column;
    }

}
