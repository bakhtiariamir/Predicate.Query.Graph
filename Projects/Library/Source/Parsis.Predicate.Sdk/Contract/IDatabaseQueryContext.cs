﻿namespace Parsis.Predicate.Sdk.Contract;

public interface IDatabaseQueryContext : IQueryContext
{
    ICacheInfoCollection CacheInfoCollection
    {
        get;
    }
}
