namespace Parsis.Predicate.Sdk.Contract;

public interface IMemoryCacheQueryContext : IQueryContext
{
    ICacheInfoCollection CacheInfoCollection
    {
        get;
    }
}
