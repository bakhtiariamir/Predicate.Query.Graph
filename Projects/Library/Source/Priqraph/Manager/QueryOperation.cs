using Priqraph.Builder;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Query;
using Priqraph.Setup;

namespace Priqraph.Manager;

internal class QueryOperation<TObject, TResult> : IQueryOperation<TObject, TResult> where TObject : IQueryableObject where TResult : IQueryResult
{
    private readonly ICacheInfoCollection _cacheInfoCollection;
    private readonly QueryProvider _provider;

    protected IQueryObject<TObject>? QueryObject
    {
        get;
        set;
    }

    public QueryOperation(ICacheInfoCollection cacheInfoCollection, QueryProvider provider)
    {
        _cacheInfoCollection = cacheInfoCollection;
        _provider = provider;
    }

    public void Init(IQueryObject<TObject> queryObject) => QueryObject = queryObject;

    private Task<bool> ValidateAsync() => Task.FromResult(true);

    public virtual async Task<TResult> RunAsync()
    {
        if (QueryObject is null)
            throw new ArgumentNullException(nameof(QueryObject), $"{nameof(QueryObject)} can not be null.");

        QueryObject = QueryObjectReducer<TObject>.Init(QueryObject).Reduce().Return();

        var validateQuery = await ValidateAsync();
        if (validateQuery)
        {
            var query = await QueryBuilder<TObject, TResult>.Init(_cacheInfoCollection, _provider).BuildAsync();
            return await query.Build(QueryObject);
        }

        throw new System.Exception("database query is not valid"); //ToDo


    }
}
