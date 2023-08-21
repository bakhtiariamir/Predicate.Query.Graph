using Priqraph.Contract;

namespace Priqraph.Builder.Cache;

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
