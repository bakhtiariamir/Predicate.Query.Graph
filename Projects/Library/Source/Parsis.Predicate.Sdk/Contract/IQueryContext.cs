using Parsis.Predicate.Sdk.Setup;

namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryContext
{
    ICacheInfoCollection CacheInfoCollection
    {
        get;
        set;
    }
    void UpdateCacheObjectInfo();
}
