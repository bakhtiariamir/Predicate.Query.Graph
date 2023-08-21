using Priqraph.Setup;

namespace Priqraph.Contract;

public interface ICacheInfoCollection
{
    QuerySetting QuerySetting
    {
        get;
        set;
    }
    IDictionary<string, object> Cache
    {
        get;
        set;
    }

    void InitCache(string key, object value);

    bool RemoveCache(string key);

    bool TryGet(string key, out object? value);

    bool TryGetFirst(string key, out object? value);
}
