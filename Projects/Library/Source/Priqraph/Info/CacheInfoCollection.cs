using Priqraph.Contract;
using Priqraph.Setup;

namespace Priqraph.Info
{
    public class CacheInfoCollection : ICacheInfoCollection
    {
        public QuerySetting QuerySetting
        {
            get;
            set;
        }

        public IDictionary<string, object> Cache
        {
            get;
            set;
        }

        public CacheInfoCollection(QuerySetting querySetting)
        {
            QuerySetting = querySetting;
            Cache = new Dictionary<string, object>();
        }

        public void InitCache(string key, object value) => Cache.Add(key, value);

        public bool RemoveCache(string key) => Cache.Remove(key);

        public bool TryGet(string key, out object? value) => Cache.TryGetValue(key, out value);

        public bool TryGetFirst(string key, out object? value)
        {
            value = default;
            var firstKey = Cache.Keys.FirstOrDefault(item => item.Split(":")[1] == key);
            if (string.IsNullOrWhiteSpace(firstKey)) return false;

            return Cache.TryGetValue(firstKey, out value);
        }
    }
}
