using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal class QueryOperation<TObject, TResult> : IQueryOperation<TObject, TResult> where TObject : IQueryableObject where TResult : IQueryResult
{
    private bool Validate() => true;

    public virtual TResult Run(IQueryObject<TObject> queryObject, IQuery<TObject, TResult> query)
    {
        if (queryObject is null)
            throw new ArgumentNullException(nameof(queryObject), $"{nameof(queryObject)} can not be null.");

        queryObject = QueryObjectReducer<TObject>.Init(queryObject).Reduce().Return();

        var validateQuery = Validate();
        if (validateQuery)
        {
            return query.Build(queryObject);
        }

        throw new System.Exception("database query is not valid"); //ToDo


    }
}
