using Priqraph.Contract;

namespace Priqraph.Builder.Database;

public class DatabaseQueryContext : QueryContext, IDatabaseQueryContext
{
    public DatabaseQueryContext(ICacheInfoCollection cacheCacheInfoCollection) : base(cacheCacheInfoCollection)
    {

    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
