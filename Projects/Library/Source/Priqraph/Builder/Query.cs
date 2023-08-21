using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Builder;

public abstract class Query<TObject, TQueryResult> : IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    public abstract Task<TQueryResult> Build(QueryObject<TObject> query);
}
