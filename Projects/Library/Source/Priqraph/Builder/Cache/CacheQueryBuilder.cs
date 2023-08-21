using Priqraph.Contract;

namespace Priqraph.Builder.Cache;

public abstract class CacheQueryBuilder<TObject> : QueryBuilder<TObject, CacheQueryPartCollection> where TObject : IQueryableObject
{
}
