using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal class QueryOperation<TObject, TResult> : IQueryOperation<TObject, TResult> where TObject : IQueryableObject where TResult : IQueryResult
{
    private bool Validate() => true;

    private bool ValidateQuery() => true;

    public virtual TResult Run(IQuery<TObject> query, IQueryObject<TObject, TResult> queryObject)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query), $"{nameof(query)} can not be null.");

        query = QueryReducer<TObject>.Init(query).Reduce().Return();

        var validateQuery = Validate();
        if (validateQuery)
        {
            return queryObject.Build(query);
        }

        throw new System.Exception("database query is not valid"); //ToDo
    }

    public virtual TResult RunQuery(IQuery<TObject> query, IQueryObject<TObject, TResult> queryObject)
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
