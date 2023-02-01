using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;

public abstract class QueryBuilder<TObject, TResult> : IQueryBuilder<TObject, TResult> where TObject : IQueryableObject
{
    public abstract Task<IQuery<TObject, TResult>> BuildAsync();
}
