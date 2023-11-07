using Priqraph.Contract;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQueryBuilder<TObject> : QueryBuilder<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
}
