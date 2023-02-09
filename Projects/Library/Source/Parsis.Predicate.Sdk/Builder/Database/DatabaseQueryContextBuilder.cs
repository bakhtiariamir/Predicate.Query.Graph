using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class DatabaseQueryContextBuilder : QueryContextBuilder
{
    private readonly ICacheInfoCollection _info;

    public DatabaseQueryContextBuilder(ICacheInfoCollection info) => _info = info;

    public override async Task<IQueryContext> Build() => await Task.FromResult(new DatabaseQueryContext(_info));
}
