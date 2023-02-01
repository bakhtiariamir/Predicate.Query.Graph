using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Contract;

public interface IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    Task<TQueryResult> Build(QueryObject<TObject> query);
}
