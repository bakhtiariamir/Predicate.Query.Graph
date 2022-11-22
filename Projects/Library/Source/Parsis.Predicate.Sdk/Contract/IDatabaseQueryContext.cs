namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseQueryContext : IQueryContext
{
    IDatabaseCacheInfoCollection DatabaseCacheInfoCollection
    {
        get;
    }
}
