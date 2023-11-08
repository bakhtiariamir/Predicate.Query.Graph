using Priqraph.Query.Builders;

namespace Priqraph.Generator.Cache;

public class CachePagingClauseQueryPart : CacheQueryPart<PageClause>
{

    public CachePagingClauseQueryPart(int pageNumber, int pageRows)
    {
        Parameter = new PageClause(pageNumber, pageRows);
    }

    public CachePagingClauseQueryPart(PageClause pagination)
    {
        Parameter = pagination;
    }

    public static CachePagingClauseQueryPart Create(PageClause pagination) => new(pagination);

    public static CachePagingClauseQueryPart Create(int pageSize, int pageRows) => new(pageSize, pageRows);
}
