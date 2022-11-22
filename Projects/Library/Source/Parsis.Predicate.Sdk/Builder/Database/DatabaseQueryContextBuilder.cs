using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class DatabaseQueryContextBuilder<TObject> : QueryContextBuilder<TObject> where TObject : class
{
    private readonly IDatabaseCacheInfoCollection _info;
    public DatabaseQueryContextBuilder(IDatabaseCacheInfoCollection info) => _info = info;

    public override async Task<IQueryContext> Build()
    {
        return await Task.FromResult(new DatabaseQueryContext(_info));
    }
}

