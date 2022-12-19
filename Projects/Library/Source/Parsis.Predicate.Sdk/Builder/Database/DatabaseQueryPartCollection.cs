using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class DatabaseQueryPartCollection<TObject> where TObject : IQueryableObject
{
    public IDatabaseObjectInfo DatabaseObjectInfo
    {
        get;
        set;
    }
    public DatabaseColumnsClauseQueryPart? Columns
    {
        get;
        set;
    }

    public DatabaseWhereClauseQueryPart? WhereClause
    {
        get;
        set;
    }

    public DatabaseOrdersByClauseQueryPart? OrderByClause
    {
        get;
        set;
    }

    public DatabaseJoinsClauseQueryPart? JoinClause
    {
        get;
        set;
    }

    public DatabasePagingClauseQueryPart? Paging
    {
        get;
        set;
    }

    public DatabaseGroupByClauseQueryPart? GroupByClause
    {
        get;
        set;
    }
}
