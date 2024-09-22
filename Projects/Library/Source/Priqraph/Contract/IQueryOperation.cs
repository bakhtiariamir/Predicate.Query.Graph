namespace Priqraph.Contract;

public interface IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    TResult Run(IQuery<TObject> query, IQueryObject<TObject, TResult> queryObject);
}
