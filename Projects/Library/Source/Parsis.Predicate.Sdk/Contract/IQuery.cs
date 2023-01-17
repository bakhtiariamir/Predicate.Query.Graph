using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Contract;

public interface IQuery<TObject, TQueryType, TQueryResult> where TObject : IQueryableObject
    where TQueryType : Enum
{
    Task<TQueryResult> Build(QueryObject<TObject, TQueryType> query);
}
