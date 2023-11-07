using Priqraph.Builder.Cache;
using Priqraph.Contract;
using Priqraph.Query;

namespace Priqraph.Manager;

internal abstract class CacheQueryOperation<TObject> : QueryOperation<TObject, CacheQueryResult> where TObject : IQueryableObject
{
    public override async Task<CacheQueryResult> RunAsync()
    {
        if (QueryObject is null)
            throw new ArgumentNullException(nameof(QueryObject), $"{nameof(QueryObject)} can not be null.");

        QueryObject = QueryObjectReducer<TObject>.Init(QueryObject).Reduce().Return();
        var validateQuery = await ValidateAsync();
        if (validateQuery)
            return await RunQueryAsync();

        throw new System.Exception("Cache query is not valid"); //ToDo
    }

    protected abstract Task<CacheQueryResult> RunQueryAsync();
}
