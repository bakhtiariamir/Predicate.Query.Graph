using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal class QueryOperation<TObject, TResult> : IQueryOperation<TObject, TResult> where TObject : IQueryableObject where TResult : IQueryResult
{

    private readonly IQuery<TObject, TResult> _query;


    protected IQueryObject<TObject>? QueryObject
    {
        get;
        set;
    }


    public QueryOperation(IQuery<TObject, TResult> query)
    {
        _query = query;
    }

    public void Init(IQueryObject<TObject> queryObject) => QueryObject = queryObject;

    private bool Validate() => true;

    public virtual TResult RunAsync()
    {
        if (QueryObject is null)
            throw new ArgumentNullException(nameof(QueryObject), $"{nameof(QueryObject)} can not be null.");

        QueryObject = QueryObjectReducer<TObject>.Init(QueryObject).Reduce().Return();

        var validateQuery = Validate();
        if (validateQuery)
        {
            return _query.Build(QueryObject);
        }

        throw new System.Exception("database query is not valid"); //ToDo


    }
}
