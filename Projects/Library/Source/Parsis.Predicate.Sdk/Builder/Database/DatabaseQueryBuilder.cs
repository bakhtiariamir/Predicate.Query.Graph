using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Builder.Database;
public abstract class DatabaseQueryBuilder<TObject> : QueryBuilder<TObject, DatabaseQueryOperationType, DatabaseQueryPartCollection> where TObject : IQueryableObject
{

}
