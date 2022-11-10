namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryOperation<TObject, TResult, in TOperationType> where TObject : class where TOperationType : Enum
{
    Task<TResult> RunAsync(TOperationType operationType);
}
