using Parsis.Predicate.Sdk.Builder.Cache;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Manager.Cache;

public abstract class CacheQueryOperation<TObject> : QueryOperation<TObject, CacheQueryPartCollection> where TObject : IQueryableObject
{
    public override async Task<CacheQueryPartCollection> RunAsync(QueryObject<TObject> queryObject)
    {
        QueryObject = queryObject ?? throw new System.Exception("Asd");

        QueryObject = QueryObjectReducer<TObject>.Init(queryObject).Reduce().Return();

        var validateQuery = await ValidateAsync();
        if (validateQuery)
            return await RunQueryAsync();

        throw new System.Exception("Cache query is not valid"); //ToDo
    }

    protected abstract Task<CacheQueryPartCollection> RunQueryAsync();
}
