using System;
using System.Collections.Concurrent;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Info;

public class DatabaseCacheInfoCollection : CacheInfoCollection<IDatabaseObjectInfo>, IDatabaseCacheInfoCollection
{
    private ConcurrentDictionary<string, IDatabaseObjectInfo> _cache;

    protected override string CacheKey => "cache.database";

    public DatabaseCacheInfoCollection() => _cache = new ConcurrentDictionary<string, IDatabaseObjectInfo>();

    public override IDatabaseObjectInfo InitCache(string objectType, IDatabaseObjectInfo value) => _cache.GetOrAdd(GetKey(objectType), value);

    public override bool TryRemove(string objectType, out IDatabaseObjectInfo? value) => _cache.TryRemove(GetKey(objectType), out value);

    public override bool RemoveCache(string objectType) => _cache.TryRemove(GetKey(objectType), out _);

    public override bool TryGet(string objectType, out IDatabaseObjectInfo? value) => _cache.TryGetValue(GetKey(objectType), out value);

    public override string GetKey(string objectType) => $"{CacheKey}.{objectType}";
}
