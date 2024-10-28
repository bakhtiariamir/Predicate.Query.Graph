namespace Priqraph.Contract;

public interface IQueryOperation<TObject, TQueryObject, TResult, TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
{
    TResult Run(TQueryObject query, IQueryObject<TObject,TQueryObject, TResult, TEnum> queryObject);
}
