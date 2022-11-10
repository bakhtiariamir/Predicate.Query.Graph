namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryContext<TObject> where TObject : class
{
    void UpdateCacheObjectInfo<TObjectInfo>();
}
