using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal abstract class DatabaseQueryOperation<TObject> : QueryOperation<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
    public override async Task<DatabaseQueryResult> RunAsync()
    {
        if (QueryObject is null)
            throw new ArgumentNullException(nameof(QueryObject), $"{nameof(QueryObject)} can not be null.");

        QueryObject = QueryObjectReducer<TObject>.Init(QueryObject).Reduce().Return();

        var validateQuery = await ValidateAsync();
        if (validateQuery)
            return await RunQueryAsync();

        throw new System.Exception("database query is not valid"); //ToDo
    }

    protected abstract Task<DatabaseQueryResult> RunQueryAsync();
}
