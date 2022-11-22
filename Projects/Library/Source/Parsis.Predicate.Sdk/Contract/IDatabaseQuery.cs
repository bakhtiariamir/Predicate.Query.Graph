using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseQuery<TObject> : IQuery<TObject, DatabaseQueryOperationType, DatabaseQueryPartCollection> where TObject : class
{
    Task GenerateColumn();

    Task GenerateWhereClause();

    Task GeneratePagingClause();

    Task GenerateOrderByClause();

    Task GenerateJoinClause();

    Task GenerateGroupByClause();

}
