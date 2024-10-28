using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Manager;
using Priqraph.Query;
using Priqraph.Setup;

namespace Priqraph;
public static class Operation
{
    public static IQueryOperation<TObject, TQueryObject, TResult, TEnum> SetupOperation<TObject, TQueryObject, TResult, TEnum>() 
        where TObject : IQueryableObject
        where TResult : IQueryResult
        where TQueryObject : IQuery<TObject, TEnum>
        where TEnum : struct, IConvertible
        => new QueryOperation<TObject, TQueryObject, TResult, TEnum>();

    private static IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetupQueryBuilder<TObject, TQueryObject, TEnum>() 
        where TObject : IQueryableObject
        where TQueryObject : IQuery<TObject, TEnum>
        where TEnum : struct, IConvertible
        => new QueryBuilder<TObject, TQueryObject, TEnum>();

    public static TResult Build<TObject, TQueryObject, TResult, TEnum>(this IQueryable query, QueryProvider provider, IQueryObject<TObject, TQueryObject, TResult, TEnum> queryObjectProvider, TEnum operationType) 
        where TObject : IQueryableObject
        where TResult : IQueryResult
        where TQueryObject : IQuery<TObject, TEnum>
        where TEnum : struct, IConvertible
    {
        var queryBuilder = SetupQueryBuilder<TObject, TQueryObject, TEnum>();
        //TODO
        queryBuilder.Init(operationType, provider, default);
        queryBuilder.SetQuery(query);
        return new QueryOperation<TObject, TQueryObject, TResult, TEnum>().RunQuery(queryBuilder.Generate(), queryObjectProvider);
    }
}
