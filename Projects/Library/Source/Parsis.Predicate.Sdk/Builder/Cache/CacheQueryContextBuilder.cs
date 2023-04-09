using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public class CacheQueryContextBuilder : QueryContextBuilder
{
    private readonly ICacheInfoCollection _info;

    public CacheQueryContextBuilder(ICacheInfoCollection info) => _info = info;

    public override async Task<IQueryContext> Build() => await Task.FromResult(new CacheQueryContext(_info));
}
