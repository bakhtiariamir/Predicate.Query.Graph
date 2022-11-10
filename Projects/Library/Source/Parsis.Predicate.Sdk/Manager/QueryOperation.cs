using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Manager;
public abstract class QueryOperation<TObject, TResult, TOperationType> : IQueryOperation<TObject, TResult, TOperationType> where TObject : class where TOperationType : Enum
{
    public abstract Task<TResult> RunAsync(TOperationType operationType);
}


