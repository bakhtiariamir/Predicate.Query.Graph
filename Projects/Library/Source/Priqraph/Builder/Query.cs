using Priqraph.Contract;

namespace Priqraph.Builder;

internal abstract class Query<TObject, TQueryResult> : IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    public abstract Task<TQueryResult> Build(IQueryObject<TObject> query);
}
