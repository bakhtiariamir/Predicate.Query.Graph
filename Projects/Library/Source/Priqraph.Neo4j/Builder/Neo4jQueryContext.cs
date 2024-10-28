using Priqraph.Builder;
using Priqraph.Contract;

namespace Priqraph.Neo4j.Builder;

public class Neo4jQueryContext : QueryContext, INeo4jQueryContext
{
    public Neo4jQueryContext(ICacheInfoCollection cacheCacheInfoCollection) : base(cacheCacheInfoCollection)
    {

    }

    public override void UpdateCacheObjectInfo()
    {
    }
}