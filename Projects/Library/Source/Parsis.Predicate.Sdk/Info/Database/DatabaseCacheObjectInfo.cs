using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;
public class DatabaseCacheObjectInfo<TObject> : CacheObjectInfo<IDatabaseObjectInfo<TObject>, TObject>, IDatabaseCacheObjectInfo<TObject> where TObject : class
{
    public DatabaseCacheObjectInfo(IMemoryCache memoryCache) : base(memoryCache)
    {
    }

    protected override ObjectInfoType ObjectInfoType => ObjectInfoType.Database;
}
