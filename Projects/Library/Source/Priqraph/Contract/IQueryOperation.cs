namespace Priqraph.Contract;

public interface IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    //ToDo : Change QueryObject to IQueryObject
    Task<TResult> RunAsync();

    void Init(IQueryObject<TObject> queryObject);
}
