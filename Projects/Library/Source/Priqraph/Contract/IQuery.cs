using Priqraph.Query;

namespace Priqraph.Contract;

public interface IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    Task<TQueryResult> Build(QueryObject<TObject> query);
}
