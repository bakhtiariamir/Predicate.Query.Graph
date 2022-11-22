using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public class DatabaseJoinsClauseQueryPart<TObject> : DatabaseQueryPart<TObject, IColumnPropertyInfo> where TObject : class
{
    private string? _text;
    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    protected override QueryPartType QueryPartType => QueryPartType.Columns;

    public DatabaseJoinsClauseQueryPart() { }

    DatabaseJoinsClauseQueryPart(string? text) => _text = text;

    DatabaseJoinsClauseQueryPart(string? text, IColumnPropertyInfo property)
    {
        _text = text;
        Parameters = new[] { property };
    }

    DatabaseJoinsClauseQueryPart(IColumnPropertyInfo property) => Parameters = new[] { property };

    DatabaseJoinsClauseQueryPart(IList<IColumnPropertyInfo> properties) =>  Parameters = properties;

    private string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}] As {item.Alias ?? item.GetCombinedAlias()}";

    private string SetText() => _text = string.Join(", ", Parameters.Select(SetColumnName));

    DatabaseJoinsClauseQueryPart(string? text, IList<IColumnPropertyInfo> properties)
    {
        _text = text;
        Parameters = properties;
    }

    public static DatabaseJoinsClauseQueryPart<TObject> Create(string? text) => new(text);
    public static DatabaseJoinsClauseQueryPart<TObject> Create(params IColumnPropertyInfo[] properties) => new(properties);

    public static DatabaseJoinsClauseQueryPart<TObject> Create(string? text, params IColumnPropertyInfo[] properties) => new(text, properties);
    public static DatabaseJoinsClauseQueryPart<TObject> CreateMerged(string? text, params DatabaseJoinsClauseQueryPart<TObject>[] sqlClauses) => new(text, sqlClauses.SelectMany(properties => properties.Parameters).ToList());
    public static DatabaseJoinsClauseQueryPart<TObject> CreateMerged(string? text, IEnumerable<DatabaseJoinsClauseQueryPart<TObject>> sqlQueries) => new(text, sqlQueries.SelectMany(properties => properties.Parameters).ToList());

    public static DatabaseJoinsClauseQueryPart<TObject> CreateMerged(IEnumerable<DatabaseJoinsClauseQueryPart<TObject>> sqlQueries)
    {
        var column = new DatabaseJoinsClauseQueryPart<TObject>(sqlQueries.SelectMany(properties => properties.Parameters).DistinctBy(item => new
        {
            item.Schema,
            item.DataSet,
            item.Name,
            item.ColumnName
        }).ToList()).SetText();

        return new(column);
    }

}
