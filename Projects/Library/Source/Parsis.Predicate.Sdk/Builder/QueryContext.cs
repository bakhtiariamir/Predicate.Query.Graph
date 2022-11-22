using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryContext : IQueryContext
{
    public abstract void UpdateCacheObjectInfo();
}
