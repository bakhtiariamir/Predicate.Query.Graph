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

    private static IQueryObjectBuilder<TObject> SetupQueryBuilder<TObject>() where TObject : IQueryableObject => new QueryBuilder<TObject>();

    public static TResult Build<TObject, TResult>(this IQueryable query, QueryProvider provider, ICacheInfoCollection cacheInfoCollection, IQueryObject<TObject, TResult> queryObjectProvider) where TObject : IQueryableObject
        where TResult : IQueryResult
    {
        var queryBuilder = SetupQueryBuilder<TObject>();
        queryBuilder.Init(QueryOperationType.GetData, provider, null);
        queryBuilder.SetQuery(query);
        return new QueryOperation<TObject, TResult>().RunQuery(queryBuilder.Generate(), queryObjectProvider);
    }
}
