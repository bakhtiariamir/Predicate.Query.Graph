using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal sealed class QueryOperation<TObject, TQueryObject, TResult, TEnum> : IQueryOperation<TObject, TQueryObject, TResult, TEnum> 
    where TObject : IQueryableObject 
    where TResult : IQueryResult
    where TQueryObject : IQuery<TObject, TEnum> 
    where TEnum : struct, IConvertible
{
    private bool Validate() => true;

    private bool ValidateQuery() => true;

    public TResult Run(TQueryObject query, IQueryObject<TObject, TQueryObject, TResult, TEnum> queryObject)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query), $"{nameof(query)} can not be null.");

        query = QueryReducer<TObject, TQueryObject, TEnum>.Init(query).Reduce().Return();

        var validateQuery = Validate();
        if (validateQuery)
        {
            return queryObject.Build(query);
        }

        throw new System.Exception("database query is not valid"); //ToDo
    }

    public TResult RunQuery(TQueryObject query, IQueryObject<TObject, TQueryObject, TResult, TEnum> queryObject)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query), $"{nameof(query)} can not be null.");

        var validateQuery = ValidateQuery();
        if (validateQuery)
        {
            return queryObject.Build(query);
        }

        throw new System.Exception("database query is not valid"); //ToDo
    }
}
