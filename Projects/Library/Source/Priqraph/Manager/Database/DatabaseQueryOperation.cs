using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager.Database;

public abstract class DatabaseQueryOperation<TObject> : QueryOperation<TObject, DatabaseQueryPartCollection> where TObject : IQueryableObject
{
    public override async Task<DatabaseQueryPartCollection> RunAsync(QueryObject<TObject> queryObject)
    {
        QueryObject = queryObject ?? throw new System.Exception("Asd");

        QueryObject = QueryObjectReducer<TObject>.Init(queryObject).Reduce().Return();

        var validateQuery = await ValidateAsync();
        if (validateQuery)
            return await RunQueryAsync();

        throw new System.Exception("database query is not valid"); //ToDo
    }

    protected abstract Task<DatabaseQueryPartCollection> RunQueryAsync();
}
