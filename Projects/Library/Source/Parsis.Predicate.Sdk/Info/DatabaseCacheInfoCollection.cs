using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Info;

public class DatabaseCacheInfoCollection : CacheInfoCollection, ICacheInfoCollection
{
    //public override TObjectInfo CastObject<TObjectInfo, TPropertyInfo>(object value) 
    //{
    //    if (value is IDatabaseObjectInfo info)
    //        return (TObjectInfo)info;

    //    throw new InvalidCastException();//todo
    //}
    //public override void InitCache(string objectType, IDatabaseObjectInfo value) => throw new NotImplementedException();

    //public override bool TryRemove(string objectType, out IDatabaseObjectInfo? value) => throw new NotImplementedException();

    //public override bool RemoveCache(string key) => _cache.TryRemove(GetKey(key), out _);

    //public override bool TryGet(string objectType, out IDatabaseObjectInfo? value) => throw new NotImplementedException();

    //public override string GetKey(string key) => $"{CacheKey}.{key}";

    //public void InitCache(string key, IDatabaseObjectInfo value) => _cache.GetOrAdd(GetKey(key), value);

    //public bool TryRemove(string key, out IDatabaseObjectInfo? value) => _cache.TryRemove(GetKey(key), out value);

    //public bool TryGet(string key, out IDatabaseObjectInfo? value) => _cache.TryGetValue(GetKey(key), out value);
}
