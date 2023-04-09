using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public class MemoryCacheQueryBuilder<TObject> : CacheQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly CacheQueryContextBuilder _contextBuilder;

    private IQueryContext? _queryContext;

    private IQuery<TObject, CacheQueryPartCollection>? _query;

    private MemoryCacheQueryBuilder(ICacheInfoCollection info) => _contextBuilder = new CacheQueryContextBuilder(info);

    public static MemoryCacheQueryBuilder<TObject> Init(ICacheInfoCollection info) => new(info);

    public async Task<MemoryCacheQueryBuilder<TObject>> InitContextAsync()
    {
        _queryContext = await _contextBuilder.Build();
        return this;
    }

    public Task<MemoryCacheQueryBuilder<TObject>> InitQueryAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asdas"); //ToDo

        _query = new MemoryCacheQuery<TObject>(_queryContext);
        return Task.FromResult(this);
    }

    public override Task<IQuery<TObject, CacheQueryPartCollection>> BuildAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asd"); //ToDo : Exception

        if (_query == null)
            throw new System.Exception("asda"); //ToDo

        return Task.FromResult(_query);
    }
}
