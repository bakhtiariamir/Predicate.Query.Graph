using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IObjectQueryGenerator<TParameter, out TObjectQuery, in TQueryResult> 
    where TObjectQuery : IObjectQuery<TParameter>
    where TQueryResult : IQueryResult
{
    TObjectQuery? GenerateResult(QueryOperationType operationType, TQueryResult query);
}
