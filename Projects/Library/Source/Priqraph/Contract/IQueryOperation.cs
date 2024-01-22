namespace Priqraph.Contract;

public interface IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    TResult Run(IQueryObject<TObject> queryObject, IQuery<TObject, TResult> query);
}
