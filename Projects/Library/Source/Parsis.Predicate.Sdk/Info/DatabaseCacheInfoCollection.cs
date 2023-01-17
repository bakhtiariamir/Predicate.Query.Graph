using Parsis.Predicate.Sdk.Contract;
using System;
using System.Collections.Concurrent;

namespace Parsis.Predicate.Sdk.Info;

public class DatabaseCacheInfoCollection : CacheInfoCollection<IDatabaseObjectInfo>, IDatabaseCacheInfoCollection
{
    private ConcurrentDictionary<string, IDatabaseObjectInfo> _cache;

    protected override string CacheKey => "cache.database";

    public DatabaseCacheInfoCollection() => _cache = new ConcurrentDictionary<string, IDatabaseObjectInfo>();

    public override void InitCache(string key, IDatabaseObjectInfo value) => _cache.GetOrAdd(GetKey(key), value);

    public override bool TryRemove(string key, out IDatabaseObjectInfo? value) => _cache.TryRemove(GetKey(key), out value);

    public override bool RemoveCache(string key) => _cache.TryRemove(GetKey(key), out _);

    public override bool TryGet(string key, out IDatabaseObjectInfo? value) => _cache.TryGetValue(GetKey(key), out value);

    public override string GetKey(string key) => $"{CacheKey}.{key}";
}
