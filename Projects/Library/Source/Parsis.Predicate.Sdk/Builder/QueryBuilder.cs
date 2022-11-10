using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryBuilder<TObject, TResult> : IQueryBuilder<TObject, TResult> where TObject : class
{
    public abstract Task<IQuery<TObject, TResult>> Build(QueryObject<TObject> query);
}

