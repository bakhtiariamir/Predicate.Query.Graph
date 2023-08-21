using Priqraph.Query;

namespace Priqraph.Generator.Cache;

public class CachePagingClauseQueryPart : CacheQueryPart<Page>
{

    public CachePagingClauseQueryPart(int pageNumber, int pageRows)
    {
        Parameter = new Page(pageNumber, pageRows);
    }

    public CachePagingClauseQueryPart(Page pagination)
    {
        Parameter = pagination;
    }

    public static CachePagingClauseQueryPart Create(Page pagination) => new(pagination);

    public static CachePagingClauseQueryPart Create(int pageSize, int pageRows) => new(pageSize, pageRows);
}
