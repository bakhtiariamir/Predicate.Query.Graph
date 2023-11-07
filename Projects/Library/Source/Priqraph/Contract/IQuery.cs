namespace Priqraph.Contract;

public interface IQuery<TObject, TQueryResult> where TObject : IQueryableObject
{
    Task<TQueryResult> Build(IQueryObject<TObject> query);
}
