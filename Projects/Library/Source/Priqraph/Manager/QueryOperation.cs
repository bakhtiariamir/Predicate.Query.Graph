using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query;

namespace Priqraph.Manager;

public abstract class QueryOperation<TObject, TResult> : IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    protected QueryObject<TObject>? QueryObject
    {
        get;
        set;
    }

    public QueryObjectBuilder<TObject>? QueryBuilder
    {
        get;
        private set;
    }

    public void Init(QueryOperationType queryOperationType)
    {
        QueryObject = QueryObject<TObject>.Init(queryOperationType);
        QueryBuilder = QueryObjectBuilder<TObject>.Init(QueryObject);
    }

    protected abstract Task<bool> ValidateAsync();

    public abstract Task<TResult> RunAsync(QueryObject<TObject> queryObject);
}
