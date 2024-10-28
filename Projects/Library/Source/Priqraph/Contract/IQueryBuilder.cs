namespace Priqraph.Contract;

public interface IQueryBuilder<TObject, TQueryObject, out TResult, TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
{
    IQueryObject<TObject, TQueryObject, TResult, TEnum> Build(bool isObjectQuery = true);
}
