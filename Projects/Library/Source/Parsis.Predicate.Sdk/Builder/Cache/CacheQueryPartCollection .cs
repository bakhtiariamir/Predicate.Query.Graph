using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Generator.Cache;

namespace Parsis.Predicate.Sdk.Builder.Cache;

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