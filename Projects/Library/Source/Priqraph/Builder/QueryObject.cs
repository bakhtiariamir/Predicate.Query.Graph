using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class QueryObject<TObject, TQueryResult> : IQueryObject<TObject, TQueryResult> where TObject : IQueryableObject
{
    public abstract TQueryResult Build(IQuery<TObject> query);
}
