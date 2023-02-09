using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public abstract class DatabaseQueryBuilder<TObject> : QueryBuilder<TObject, DatabaseQueryPartCollection> where TObject : IQueryableObject
{
}
