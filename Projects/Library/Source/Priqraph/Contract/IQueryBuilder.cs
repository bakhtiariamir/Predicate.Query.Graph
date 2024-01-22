namespace Priqraph.Contract;

public interface IQueryBuilder<TObject, out TResult> where TObject : IQueryableObject
{
    IQuery<TObject, TResult> Build();
}
