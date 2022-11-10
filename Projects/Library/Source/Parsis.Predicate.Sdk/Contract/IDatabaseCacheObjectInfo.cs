namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseCacheObjectInfo<TObject> : ICacheObjectInfo<IDatabaseObjectInfo<TObject>, TObject> where TObject : class
{

}
