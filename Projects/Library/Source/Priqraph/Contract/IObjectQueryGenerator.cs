using Priqraph.Builder.Database;
using Priqraph.DataType;

namespace Priqraph.Contract;

public interface IObjectQueryGenerator<TParameter, out TObjectQuery, in TQueryResult> where TObjectQuery : IObjectQuery<TParameter>
    where TQueryResult : IQueryResult
{
    TObjectQuery? GenerateResult(DatabaseQueryOperationType operationType, TQueryResult query);
}
