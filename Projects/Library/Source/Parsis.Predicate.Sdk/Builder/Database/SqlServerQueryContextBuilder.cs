using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQueryContextBuilder<TObject> : DatabaseQueryContextBuilder<TObject> where TObject : class
{
    private readonly IDatabaseCacheObjectInfo<TObject> _info;

    public SqlServerQueryContextBuilder(IDatabaseCacheObjectInfo<TObject> info)
    {
        _info = info;
    }

    public override async Task<QueryContext<TObject>> Build()
    {
        return await _info.GenerateSqlServerQueryContext();
    }
}
