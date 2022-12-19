using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryBuilder<TObject, TQueryType, TResult> : IQueryBuilder<TObject, TQueryType, TResult> where TObject : IQueryableObject where TQueryType : Enum
{
    public abstract Task<IQuery<TObject, TQueryType, TResult>> BuildAsync();
}

