using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public class SqlServerCacheObjectInfo<TObject> : CacheObjectInfo<SqlServerObjectInfo<TObject>, TObject> where TObject : class
{
    protected override ObjectInfoType ObjectInfoType => ObjectInfoType.DatabaseSqlServer;

    public SqlServerCacheObjectInfo(IMemoryCache memoryCache) : base(memoryCache)
    {
    }
}
