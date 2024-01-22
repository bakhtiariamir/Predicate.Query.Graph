using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Manager;
using Priqraph.Query;
using Priqraph.Setup;

namespace Priqraph;
public static class Operation
{
    public static IQueryOperation<TObject, TResult> SetupOperation<TObject, TResult>() where TObject : IQueryableObject
        where TResult : IQueryResult =>
        new QueryOperation<TObject, TResult>();

    //provider switch
    //{
    //    QueryProvider.SqlServer => (IQueryOperation<TObject, TResult>)new SqlQueryOperation<TObject>(cacheInfoCollection),
    //    QueryProvider.InMemoryCache => (IQueryOperation<TObject, TResult>)new MemoryCacheQueryOperation<TObject>(cacheInfoCollection),
    //    _ or QueryProvider.RestApi or QueryProvider.SoapService or QueryProvider.Neo4J or QueryProvider.DistributedCache => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
    //};

    public static IQueryObjectBuilder<TObject> SetupQueryBuilder<TObject>(this QueryProvider provider, ICacheInfoCollection cacheInfoCollection) where TObject : IQueryableObject => new QueryObjectBuilder<TObject>(provider);

    //public static IBuilder<TObject, TMiddleware> SetupBuilder<TObject, TMiddleware>(this QueryProvider provider, ICacheInfoCollection cacheInfoCollection) where TObject : IQueryableObject where TMiddleware : IBuilderMiddleware<TObject> => Builder<TObject, TMiddleware>.Init(provider, cacheInfoCollection);

}
