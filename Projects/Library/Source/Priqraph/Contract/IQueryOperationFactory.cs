using Priqraph.Setup;

namespace Priqraph.Contract
{
    public interface IQueryOperationFactory<TObject, TQueryObject, out TResult, TEnum>
        where TObject : IQueryableObject
        where TQueryObject : IQuery<TObject, TEnum>
        where TEnum : struct, IConvertible
    {
        IQueryObject<TObject, TQueryObject, TResult, TEnum> QueryProvider(ICacheInfoCollection cacheInfoCollection, QueryProvider provider);
    }
}
