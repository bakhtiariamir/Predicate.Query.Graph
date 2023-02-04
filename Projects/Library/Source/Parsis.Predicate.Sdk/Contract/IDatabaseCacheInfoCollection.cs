namespace Parsis.Predicate.Sdk.Contract;

public interface IDatabaseCacheInfoCollection : ICacheInfoCollection
{
    void InitCache(string objectType, IDatabaseObjectInfo value);
    bool TryRemove(string objectType, out IDatabaseObjectInfo? value); 
    bool TryGet(string objectType, out IDatabaseObjectInfo? value);

}
