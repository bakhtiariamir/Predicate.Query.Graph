using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;

public abstract class Query<TObject, TQueryResult> : IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    public abstract Task<TQueryResult> Build(QueryObject<TObject> query);
}
