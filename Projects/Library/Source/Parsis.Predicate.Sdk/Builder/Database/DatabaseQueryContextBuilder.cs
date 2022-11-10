using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class DatabaseQueryContextBuilder<TObject> : QueryContextBuilder<TObject> where TObject : class
{
    private readonly IDatabaseCacheObjectInfo<TObject> _info;
    public DatabaseQueryContextBuilder(IDatabaseCacheObjectInfo<TObject> info) => _info = info;

    public override async Task<IQueryContext<TObject>> Build()
    {
        return await Task.FromResult(new DatabaseQueryContext<TObject>(_info));
    }
}

