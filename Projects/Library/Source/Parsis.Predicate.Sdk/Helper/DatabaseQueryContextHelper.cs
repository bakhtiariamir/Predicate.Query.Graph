using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Info;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseQueryContextHelper
{
    public static SqlServerQueryContext<TObject> GenerateSqlServerQueryContext<TObject>(this IMemoryCache cache) where TObject : class
    {
        var type = typeof(TObject);
        var cacheObjectInfo = new SqlServerCacheObjectInfo<TObject>(cache);
        var objectInfo = cacheObjectInfo.GetObjectInfo();
        if (objectInfo == null)
        {
            //ToDo : Create PropertyInfo lists
            //ToDo : Change ObjectAttributeHelper Name for sql server helper
            //Inject IMemoryCache into
            var table = type.TableName();
            var schema = type.TableSchemaName();
            objectInfo = new SqlServerObjectInfo<TObject>(table, schema, new List<IPropertyInfo>());
            cacheObjectInfo.SaveObjectInfo(objectInfo);
        }

        var context = new SqlServerQueryContext<TObject>(objectInfo);
        return context;
    }
}
