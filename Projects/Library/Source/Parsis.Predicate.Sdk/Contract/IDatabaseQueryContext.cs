namespace Parsis.Predicate.Sdk.Contract;

public interface IDatabaseQueryContext : IQueryContext
{
    ICacheInfoCollection DatabaseCacheInfoCollection
    {
        get;
    }
}
