using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class QueryBuilder<TObject, TResult> : IQueryBuilder<TObject, TResult> where TObject : IQueryableObject
{
    public abstract Task<IQuery<TObject, TResult>> BuildAsync();
}
