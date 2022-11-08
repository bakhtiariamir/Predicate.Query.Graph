using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class MySqlQueryGenerator<TObject> : QueryGeneratorFactory<TObject> where TObject : class
{
    private readonly IMemoryCache _memoryCache;

    protected MySqlQueryGenerator(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public override IQuery<TObject> Init(ObjectQuery<TObject> query)
    {
        return new SqlServerQuery<TObject>(_memoryCache);
    }
}

