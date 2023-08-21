using Priqraph.Builder.Database;
using Priqraph.Contract;

namespace Priqraph.Manager.Database;

public class SqlQueryOperation<TObject> : DatabaseQueryOperation<TObject> where TObject : IQueryableObject
{
    private readonly ICacheInfoCollection _databaseCacheInfoCollection;

    public SqlQueryOperation(ICacheInfoCollection databaseCacheInfoCollection)
    {
        _databaseCacheInfoCollection = databaseCacheInfoCollection;
    }

    protected override Task<bool> ValidateAsync()
    {
        return Task.FromResult(true); // todo
    }

    protected override async Task<DatabaseQueryPartCollection> RunQueryAsync()
    {
        if (QueryObject == null) throw new System.Exception("adasd"); //ToDo

        var query = await (await (await SqlServerQueryBuilder<TObject>.Init(_databaseCacheInfoCollection).InitContextAsync()).InitQueryAsync()).BuildAsync();
        return await query.Build(QueryObject);
    }
}
