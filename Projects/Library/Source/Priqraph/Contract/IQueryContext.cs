using Priqraph.Setup;

namespace Priqraph.Contract;

public interface IQueryContext
{
    ICacheInfoCollection CacheInfoCollection
    {
        get;
        set;
    }
    void UpdateCacheObjectInfo();
}
