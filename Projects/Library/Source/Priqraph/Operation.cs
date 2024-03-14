using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Manager;
using Priqraph.Query;
using Priqraph.Setup;

namespace Priqraph;
public static class Operation
{
    public static IQueryOperation<TObject, TResult> SetupOperation<TObject, TResult>() where TObject : IQueryableObject
        where TResult : IQueryResult =>
        new QueryOperation<TObject, TResult>();

    public static IQueryObjectBuilder<TObject> SetupQueryBuilder<TObject>(this QueryProvider provider, ICacheInfoCollection cacheInfoCollection) where TObject : IQueryableObject => new QueryObjectBuilder<TObject>(provider);

    public static TResult Build<TObject, TResult>(this IQueryable query, QueryProvider provider, ICacheInfoCollection cacheInfoCollection, IQuery<TObject, TResult> queryProvider) where TObject : IQueryableObject
        where TResult : IQueryResult
    {
        var queryBuilder = provider.SetupQueryBuilder<TObject>(cacheInfoCollection);
        queryBuilder.Init(QueryOperationType.GetData, QueryProvider.SqlServer, null);
        queryBuilder.SetQuery(query);
        return new QueryOperation<TObject, TResult>().RunQuery(queryBuilder.Generate(), queryProvider);
    }
}
