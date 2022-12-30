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

    protected override async Task<DatabaseQueryPartCollection> SelectAsync() => await RunQueryAsync(DatabaseQueryOperationType.Select);
    protected override async Task<DatabaseQueryPartCollection> InsertAsync() => await RunQueryAsync(DatabaseQueryOperationType.Insert);
    protected override async Task<DatabaseQueryPartCollection> UpdateAsync() => await RunQueryAsync(DatabaseQueryOperationType.Update);
    protected override async Task<DatabaseQueryPartCollection> DeleteAsync() => await RunQueryAsync(DatabaseQueryOperationType.Delete);

    private async Task<DatabaseQueryPartCollection> RunQueryAsync(DatabaseQueryOperationType queryOperationType)
    {
        if (QueryObject == null) throw new System.Exception("adasd"); //ToDo

        var query = await (await (
                    await SqlServerQueryBuilder<TObject>.Init(_databaseCacheInfoCollection).InitContextAsync())
                .InitQueryAsync(queryOperationType))
            .BuildAsync();
        return await query.Build(QueryObject);
    }
}
