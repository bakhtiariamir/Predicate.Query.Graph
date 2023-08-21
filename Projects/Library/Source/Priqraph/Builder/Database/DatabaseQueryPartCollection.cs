using Priqraph.Contract;
using Priqraph.Generator.Database;

namespace Priqraph.Builder.Database;

public class DatabaseQueryPartCollection : IQueryResult
{
    public IDatabaseObjectInfo? DatabaseObjectInfo
    {
        get;
        set;
    }

    public DatabaseCommandQueryPart? Command
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

    public DatabaseQueryPartCollection? ResultQuery
    {
        get;
        set;
    }
}

public interface IQueryResult
{
}
