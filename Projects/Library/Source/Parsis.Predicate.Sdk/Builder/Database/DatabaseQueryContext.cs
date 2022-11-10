using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class DatabaseQueryContext<TObject> : QueryContext<TObject>, IDatabaseQueryContext<TObject> where TObject : class 
{
    public IDatabaseCacheObjectInfo<TObject> Info
    {
        get;
    }

    public DatabaseQueryContext(IDatabaseCacheObjectInfo<TObject> info)
    {
        Info = info;
    }

    public override void UpdateCacheObjectInfo<TObjectInfo>()
    {
        throw new NotImplementedException();
    }
}