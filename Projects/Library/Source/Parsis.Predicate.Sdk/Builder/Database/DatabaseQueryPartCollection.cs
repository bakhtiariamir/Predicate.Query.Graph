using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class DatabaseQueryPartCollection<TObject> where TObject : class
{
    public string Main
    {
        get;
        set;
    } = string.Empty;

    public DatabaseColumnsClauseQueryPart<TObject>? Columns
    {
        get;
        set;
    }

    public string WhereClause
    {
        get;
        set;
    } = string.Empty;

    public string OrderByClause
    {
        get;
        set;
    } = string.Empty;

    public string JoinClause
    {
        get;
        set;
    } = string.Empty;

    public string Paging
    {
        get;
        set;
    } = string.Empty;

    public string GroupByClause
    {
        get;
        set;
    } = string.Empty;
}
