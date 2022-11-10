using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryContext<TObject> : IQueryContext<TObject> where TObject : class
{
    public abstract void UpdateCacheObjectInfo<TObjectInfo>();
}
