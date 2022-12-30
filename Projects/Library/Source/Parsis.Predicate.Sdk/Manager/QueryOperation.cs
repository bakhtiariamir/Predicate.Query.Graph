using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Manager;
public abstract class QueryOperation<TObject, TResult, TOperationType> : IQueryOperation<TObject, TOperationType, TResult> where TObject : IQueryableObject where TOperationType : Enum
{
    protected abstract QueryObject<TObject, TOperationType>? QueryObject
    {
        get;
        set;
    }

    protected abstract Task<bool> ValidateAsync();
    public abstract Task<TResult> RunAsync(QueryObject<TObject, TOperationType> queryObject);
}


