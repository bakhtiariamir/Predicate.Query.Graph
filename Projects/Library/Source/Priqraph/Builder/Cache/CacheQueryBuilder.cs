using Priqraph.Contract;

namespace Priqraph.Builder.Cache;

public abstract class CacheQueryBuilder<TObject> : QueryBuilder<TObject, CacheQueryResult> where TObject : IQueryableObject
{
}
