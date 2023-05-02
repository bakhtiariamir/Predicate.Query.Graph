using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public class CacheQueryContext : QueryContext, IDatabaseQueryContext
{
    public CacheQueryContext(ICacheInfoCollection cacheInfoCollection) : base(cacheInfoCollection)
    {

    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
