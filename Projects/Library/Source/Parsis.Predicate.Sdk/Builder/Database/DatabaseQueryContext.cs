﻿using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class DatabaseQueryContext : QueryContext, IDatabaseQueryContext
{
    public ICacheInfoCollection CacheInfoCollection
    {
        get;
    }

    public DatabaseQueryContext(ICacheInfoCollection cacheCacheInfoCollection)
    {
        CacheInfoCollection = cacheCacheInfoCollection;
    }

    public override void UpdateCacheObjectInfo()
    {
        throw new NotImplementedException();
    }
}
