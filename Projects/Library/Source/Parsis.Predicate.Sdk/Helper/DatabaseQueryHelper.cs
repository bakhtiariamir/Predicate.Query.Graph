using System.Text;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseQueryHelper
{
    public static IDatabaseObjectInfo? GetLastObjectInfo<TObject>(this IDatabaseCacheInfoCollection databaseCacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        if (!databaseCacheInfoCollection.TryGet(databaseCacheInfoCollection.GetKey(type.Name), out IDatabaseObjectInfo? objectInfo))
        {
            objectInfo = type.GetObjectInfo();
            databaseCacheInfoCollection.InitCache(type.Name, objectInfo);
        }

        return objectInfo;
    }

    public static string GetSelectQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : IQueryableObject
    {
        if (queryParts.Columns == null) throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);

        var select = new StringBuilder();
        select.Append($"SELECT {queryParts.Columns.Text} ");
        select.Append($"FROM {queryParts.DatabaseObjectInfo} ");
        select.Append($"{queryParts.JoinClause?.Text} ");
        select.Append(queryParts.WhereClause != null ? $"WHERE {queryParts.WhereClause.Text} " : "");
        select.Append(queryParts.GroupByClause != null ? $"GROUP BY {queryParts.GroupByClause.Text} " : "");
        select.Append(queryParts.GroupByClause is {Having: { }} ?  $"HAVING {queryParts.GroupByClause?.Having} " : "");
        select.Append(queryParts.OrderByClause != null ? $"{queryParts.OrderByClause.Text}" : "");

        return select.ToString();
    }

    public static string GetInsertQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : IQueryableObject
    {
        return string.Empty;
    }

    public static string GetUpdateQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : IQueryableObject
    {
        return string.Empty;
    }

    public static string GetDeleteQuery<TObject>(this DatabaseQueryPartCollection<TObject> queryParts) where TObject : IQueryableObject
    {
        return string.Empty;
    }
}
