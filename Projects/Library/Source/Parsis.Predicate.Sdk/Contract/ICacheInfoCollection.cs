namespace Parsis.Predicate.Sdk.Contract;

public interface ICacheInfoCollection<TObjectInfo>
{
    void InitCache(string objectType, TObjectInfo value);

    bool TryRemove(string objectType, out TObjectInfo? value);

    bool RemoveCache(string objectType);

    bool TryGet(string objectType, out TObjectInfo? value);

    string GetKey(string objectType);
}
