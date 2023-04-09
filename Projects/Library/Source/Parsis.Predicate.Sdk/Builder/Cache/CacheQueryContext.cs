using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public class CacheQueryContext : QueryContext, IDatabaseQueryContext
{
    public ICacheInfoCollection CacheInfoCollection
    {
        get;
    }

    //IMemoryCache options

    public CacheQueryContext(ICacheInfoCollection cacheCacheInfoCollection)
    {
        CacheInfoCollection = cacheCacheInfoCollection;
    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
