using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;
public abstract class CacheObjectInfo<TObjectInfo, TObject> : ICacheObjectInfo<TObjectInfo, TObject> where TObjectInfo : IObjectInfo<TObject> where TObject : class
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheCacheEntryOptions;

    protected abstract ObjectInfoType ObjectInfoType
    {
        get;
    }
    protected CacheObjectInfo(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheCacheEntryOptions = new MemoryCacheEntryOptions();
    }


    public TObjectInfo? GetObjectInfo()
    {
        var key = $"{typeof(TObject)}_{ObjectInfoType}";
        var data = _memoryCache.Get(key);
        return data != null ? (TObjectInfo)data : default(TObjectInfo);
    }

    public void SaveObjectInfo(TObjectInfo objectInfo) => _memoryCache.Set($"{typeof(TObject)}_{ObjectInfoType}", objectInfo, options: _cacheCacheEntryOptions);


}
