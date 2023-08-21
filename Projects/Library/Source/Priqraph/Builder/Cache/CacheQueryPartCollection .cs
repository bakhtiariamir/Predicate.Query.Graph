using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Generator.Cache;

namespace Priqraph.Builder.Cache;

public class CacheQueryPartCollection : IQueryResult
{
    public IObjectInfo<IPropertyInfo>? DatabaseObjectInfo
    {
        get;
        set;
    }

    public CacheCommandQueryPart? Command
    {
        get;
        set;
    }

    public CacheWhereClauseQueryPart? WhereClause
    {
        get;
        set;
    }

    public CacheOrdersByClauseQueryPart? OrderByClause
    {
        get;
        set;
    }

    public CachePagingClauseQueryPart? Paging
    {
        get;
        set;
    }
}