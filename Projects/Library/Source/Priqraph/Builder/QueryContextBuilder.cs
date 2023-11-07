using Priqraph.Builder.Cache;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.Setup;

namespace Priqraph.Builder;

internal class QueryContextBuilder : IQueryContextBuilder
{
    private readonly ICacheInfoCollection _info;
    private readonly QueryProvider _provider;
    private QueryContextBuilder(ICacheInfoCollection info, QueryProvider provider)
    {
        _info = info;
        _provider = provider;
    }

    public static QueryContextBuilder Init(ICacheInfoCollection info, QueryProvider provider) => new(info, provider);

    public async Task<IQueryContext> BuildAsync() => _provider switch {
        QueryProvider.SqlServer => await Task.FromResult(new DatabaseQueryContext(_info)),
        QueryProvider.InMemoryCache => await Task.FromResult(new CacheQueryContext(_info)),
        QueryProvider.Neo4J or
        QueryProvider.RestApi or
        QueryProvider.SoapService or
        QueryProvider.DistributedCache or
        _ => throw new ArgumentOutOfRangeException()
    };
}