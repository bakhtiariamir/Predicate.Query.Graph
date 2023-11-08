using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class Query<TObject, TQueryResult> : IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    public abstract TQueryResult Build(IQueryObject<TObject> query);
}
