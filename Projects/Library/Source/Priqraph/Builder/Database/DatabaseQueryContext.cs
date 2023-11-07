using Priqraph.Contract;

namespace Priqraph.Builder.Database;

internal class DatabaseQueryContext : QueryContext, IDatabaseQueryContext
{
    public DatabaseQueryContext(ICacheInfoCollection cacheCacheInfoCollection) : base(cacheCacheInfoCollection)
    {

    }

    public override void UpdateCacheObjectInfo()
    {
    }
}
