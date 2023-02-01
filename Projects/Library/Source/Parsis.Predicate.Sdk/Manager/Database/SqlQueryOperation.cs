using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Manager.Database;

public class SqlQueryOperation<TObject> : DatabaseQueryOperation<TObject> where TObject : IQueryableObject
{
    private readonly IDatabaseCacheInfoCollection _databaseCacheInfoCollection;

    public SqlQueryOperation(IDatabaseCacheInfoCollection databaseCacheInfoCollection)
    {
        _databaseCacheInfoCollection = databaseCacheInfoCollection;
    }

    protected override Task<bool> ValidateAsync()
    {
        return Task.FromResult(true); // todo
    }

    //protected override async Task<DatabaseQueryPartCollection> SelectAsync() => await RunQueryAsync(QueryOperationType.GetData);

    //protected override async Task<DatabaseQueryPartCollection> InsertAsync() => await RunQueryAsync(QueryOperationType.Add);

    //protected override async Task<DatabaseQueryPartCollection> UpdateAsync() => await RunQueryAsync(QueryOperationType.Edit);

    //protected override async Task<DatabaseQueryPartCollection> DeleteAsync() => await RunQueryAsync(QueryOperationType.Remove);

    protected override async Task<DatabaseQueryPartCollection> RunQueryAsync()
    {
        if (QueryObject == null) throw new System.Exception("adasd"); //ToDo

        var query = await (await (await SqlServerQueryBuilder<TObject>.Init(_databaseCacheInfoCollection).InitContextAsync()).InitQueryAsync()).BuildAsync();
        return await query.Build(QueryObject);
    }
}
