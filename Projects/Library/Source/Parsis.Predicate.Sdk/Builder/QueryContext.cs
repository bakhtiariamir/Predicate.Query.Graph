using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Setup;

namespace Parsis.Predicate.Sdk.Builder;

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
