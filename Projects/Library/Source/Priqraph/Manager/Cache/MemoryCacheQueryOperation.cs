using Priqraph.Builder.Cache;
using Priqraph.Contract;

namespace Priqraph.Manager.Cache;

public class MemoryCacheQueryOperation<TObject> : CacheQueryOperation<TObject> where TObject : IQueryableObject
{
    private readonly ICacheInfoCollection _databaseCacheInfoCollection;

    public MemoryCacheQueryOperation(ICacheInfoCollection databaseCacheInfoCollection)
    {
        _databaseCacheInfoCollection = databaseCacheInfoCollection;
    }

    protected override Task<bool> ValidateAsync()
    {
        return Task.FromResult(true); // todo
    }

    protected override async Task<CacheQueryPartCollection> RunQueryAsync()
    {
        if (QueryObject == null) throw new System.Exception("adasd"); //ToDo

        var query = await (await (await MemoryCacheQueryBuilder<TObject>.Init(_databaseCacheInfoCollection).InitContextAsync()).InitQueryAsync()).BuildAsync();
        return await query.Build(QueryObject);
    }
}
