using Parsis.Predicate.Sdk.Builder.Cache;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Cache;
using Parsis.Predicate.Sdk.Manager.Result.Cache;

namespace Parsis.Predicate.Sdk.Helper;


public static class CacheQueryHelper
{
    public static CacheQueryObject GetSelectQuery(this CacheQueryPartCollection queryParts)
    {
        if (queryParts.Command == null) throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);

        var getData = new CacheGetDataQueryObject();
        if (queryParts.WhereClause != null)
            getData.Filter = queryParts.WhereClause.Parameter.Predicate;

        if (queryParts.OrderByClause != null)
            getData.Sorts = queryParts.OrderByClause.Parameter;

        if (queryParts.Paging != null)
            getData.Page = queryParts.Paging.Parameter;


        return new CacheQueryObject
        {
            GetDataQueryObject = getData
        };
    }

    public static CacheQueryObject GetCommandQuery(this CacheQueryPartCollection queryParts)
    {
        if (queryParts.Command == null) throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);

        return queryParts.Command.OperationType switch
        {
            QueryOperationType.Add => queryParts.Command.GetAddQuery(),
            QueryOperationType.Remove => queryParts.Command.GetDeleteQuery(),
            QueryOperationType.Edit => queryParts.Command.GetUpdateQuery(),
            _ => throw new NotSupported(ExceptionCode.QueryGenerator)
        };
    }

    private static CacheQueryObject GetAddQuery(this CacheCommandQueryPart command)
    {
        var objects = (ICollection<CacheClausePredicate>)command.CommandParts["Values"];
        var type = objects.FirstOrDefault()!.GetType();
        var list = new List<CacheCommandQueryObject>();
        foreach (var objectValue in objects)
        {
            list.Add(new CacheCommandQueryObject
            {
                Key = objectValue.Key,
                Object = objectValue.Object,
                ObjectType = type
            });
        }

        return new CacheQueryObject
        {
            CommandQueryObject = list
        };
    }

    private static CacheQueryObject GetUpdateQuery(this CacheCommandQueryPart command)
    {
        var objects = (ICollection<CacheClausePredicate>)command.CommandParts["Values"];
        var type = objects.FirstOrDefault()!.GetType();
        var list = new List<CacheCommandQueryObject>();
        foreach (var objectValue in objects)
        {
            list.Add(new CacheCommandQueryObject
            {
                Key = objectValue.Key,
                Object = objectValue.Object,
                ObjectType = type
            });
        }

        return new CacheQueryObject
        {
            CommandQueryObject = list
        };
    }


    private static CacheQueryObject GetDeleteQuery(this CacheCommandQueryPart command)
    {
        var objects = (ICollection<CacheClausePredicate>)command.CommandParts["Keys"];
        var type = objects.FirstOrDefault()!.GetType();
        var list = new List<CacheCommandQueryObject>();
        foreach (var objectValue in objects)
        {
            list.Add(new CacheCommandQueryObject
            {
                Key = objectValue.Key,
                Object = objectValue.Object,
                ObjectType = type
            });
        }

        return new CacheQueryObject
        {
            CommandQueryObject = list
        };
    }
}
