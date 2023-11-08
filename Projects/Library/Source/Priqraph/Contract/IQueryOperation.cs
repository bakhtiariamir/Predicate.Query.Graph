namespace Priqraph.Contract;

public interface IQueryOperation<TObject, out TResult> where TObject : IQueryableObject
{
    TResult RunAsync();

    void Init(IQueryObject<TObject> queryObject);
}
