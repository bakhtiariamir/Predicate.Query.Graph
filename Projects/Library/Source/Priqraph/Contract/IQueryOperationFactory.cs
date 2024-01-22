using Priqraph.Setup;

namespace Priqraph.Contract
{
    public interface IQueryOperationFactory<TObject, out TResult> where TObject : IQueryableObject
    {
        IQuery<TObject, TResult> QueryProvider(ICacheInfoCollection cacheInfoCollection, QueryProvider provider);
    }
}
