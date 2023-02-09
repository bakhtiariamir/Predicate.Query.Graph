using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class DatabaseQueryContext : QueryContext, IDatabaseQueryContext
{
    public ICacheInfoCollection DatabaseCacheInfoCollection
    {
        get;
    }

    public DatabaseQueryContext(ICacheInfoCollection databaseCacheCacheInfoCollection)
    {
        DatabaseCacheInfoCollection = databaseCacheCacheInfoCollection;
    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
