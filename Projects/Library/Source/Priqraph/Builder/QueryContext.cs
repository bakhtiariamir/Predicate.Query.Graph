using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class QueryContext : IQueryContext
{
    public ICacheInfoCollection CacheInfoCollection
    {
        get;
        set;
    }

    protected QueryContext(ICacheInfoCollection cacheInfoCollection)
    {
        CacheInfoCollection = cacheInfoCollection;
    }

    public abstract void UpdateCacheObjectInfo();
}
