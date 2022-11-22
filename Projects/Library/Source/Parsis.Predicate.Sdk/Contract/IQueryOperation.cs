using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryOperation<TObject, TOperationType, TResult> where TObject : class where TOperationType : Enum
{
    Task<TResult> RunAsync(QueryObject<TObject, TOperationType> queryObject, TOperationType operationType);
}
