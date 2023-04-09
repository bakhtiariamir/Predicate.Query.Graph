using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public abstract class CacheQueryBuilder<TObject> : QueryBuilder<TObject, CacheQueryPartCollection> where TObject : IQueryableObject
{
}
