using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseQueryHelper
{
    public static IDatabaseCacheObjectInfo<TObject> GetLastObjectInfo<TObject>(this IDatabaseCacheObjectInfo<TObject> cacheObjectInfo) where TObject : class
    {
        var type = typeof(TObject);
        var objectInfo = cacheObjectInfo.GetObjectInfo();
        if (objectInfo == null)
        {
            //ToDo : Exception
            objectInfo = type.GetObjectInfo<TObject>();
            cacheObjectInfo.SaveObjectInfo(objectInfo);
        }

        return cacheObjectInfo;
    }

    public static string GetSelectQuery(this DatabaseQueryPartCollection queryParts)
    {
        return string.Empty;
    }

    public static string GetInsertQuery(this DatabaseQueryPartCollection queryParts)
    {
        return string.Empty;
    }

    public static string GetUpdateQuery(this DatabaseQueryPartCollection queryParts)
    {
        return string.Empty;
    }

    public static string GetDeleteQuery(this DatabaseQueryPartCollection queryParts)
    {
        return string.Empty;
    }
}
