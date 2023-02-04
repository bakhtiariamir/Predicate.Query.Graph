namespace Parsis.Predicate.Sdk.Contract;

public interface ICacheInfoCollection
{
    void InitCache(string objectType, object value);

    bool TryRemove(string objectType, out object? value);

    bool RemoveCache(string objectType);

    bool TryGet(string objectType, out object? value);

    string GetKey(string objectType);
}
