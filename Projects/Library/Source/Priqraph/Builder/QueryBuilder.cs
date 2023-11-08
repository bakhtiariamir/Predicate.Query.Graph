using Priqraph.Builder.Cache;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Setup;

namespace Priqraph.Builder;

internal class QueryBuilder<TObject, TResult> : IQueryBuilder<TObject, TResult> where TObject : IQueryableObject where TResult : IQueryResult
{
    private readonly QueryProvider _provider;
    private readonly IQueryContextBuilder _contextBuilder;
    private IQueryContext? _queryContext;

    private IQuery<TObject, TResult>? _query;

    private QueryBuilder(ICacheInfoCollection info, QueryProvider provider)
    {
        _provider = provider;
        _contextBuilder = QueryContextBuilder.Init(info, provider);
    }

    public static QueryBuilder<TObject, TResult> Init(ICacheInfoCollection info, QueryProvider provider) => new(info, provider); 

    public async Task<IQuery<TObject, TResult>> BuildAsync()
    { 
        _queryContext = await _contextBuilder.BuildAsync();

        if (_queryContext == null)
            throw new System.Exception("asdas"); //ToDo

        _query = _provider switch {

            QueryProvider.InMemoryCache => (IQuery<TObject, TResult>)new MemoryCacheQuery<TObject>(_queryContext),
            QueryProvider.Neo4J or
            QueryProvider.RestApi or
            QueryProvider.SoapService or
            QueryProvider.DistributedCache or
            _ => throw new ArgumentOutOfRangeException()
        };

        return _query;
    }
}
