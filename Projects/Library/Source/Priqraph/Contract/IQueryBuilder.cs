namespace Priqraph.Contract;

public interface IQueryBuilder<TObject, out TResult> where TObject : IQueryableObject
{
    IQueryObject<TObject, TResult> Build(bool isObjectQuery = true);
}
