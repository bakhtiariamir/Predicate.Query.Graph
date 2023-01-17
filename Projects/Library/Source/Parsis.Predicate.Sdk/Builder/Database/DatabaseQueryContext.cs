using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class DatabaseQueryContext : QueryContext, IDatabaseQueryContext
{
    public IDatabaseCacheInfoCollection DatabaseCacheInfoCollection
    {
        get;
    }

    public DatabaseQueryContext(IDatabaseCacheInfoCollection databaseCacheCacheInfoCollection)
    {
        DatabaseCacheInfoCollection = databaseCacheCacheInfoCollection;
    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
