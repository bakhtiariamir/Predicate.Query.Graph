namespace Parsis.Predicate.Sdk.Contract;

public interface ICacheInfoCollection
{
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
