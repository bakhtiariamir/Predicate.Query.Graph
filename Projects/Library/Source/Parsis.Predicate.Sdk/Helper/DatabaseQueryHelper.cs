using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseQueryHelper
{
    public static IDatabaseObjectInfo? GetLastObjectInfo<TObject>(this IDatabaseCacheInfoCollection databaseCacheInfoCollection) where TObject : class
    {
        var type = typeof(TObject);
        if (!databaseCacheInfoCollection.TryGet(databaseCacheInfoCollection.GetKey(type.Name), out IDatabaseObjectInfo? objectInfo))
        {
            objectInfo = type.GetObjectInfo();
            databaseCacheInfoCollection.InitCache(type.Name, objectInfo);
        }

        return objectInfo;
    }

    public static string GetSelectQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : class
    {
        return string.Empty;
    }

    public static string GetInsertQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : class
    {
        return string.Empty;
    }

    public static string GetUpdateQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : class
    {
        return string.Empty;
    }

    public static string GetDeleteQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : class
    {
        return string.Empty;
    }
}
