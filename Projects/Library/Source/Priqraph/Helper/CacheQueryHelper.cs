using Priqraph.Builder.Cache;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Cache;
using Priqraph.Manager.Result.Cache;

namespace Priqraph.Helper;


internal static class CacheQueryHelper
{
    public static CacheQueryObject GenerateSelect(this CacheQueryResult queryParts)
    {
        if (queryParts.Command == null) throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);

        var getData = new CacheGetDataQueryObject();
        if (queryParts.WhereClause != null)
            getData.Filter = queryParts.WhereClause.Parameter.Predicate;

        if (queryParts.OrderByClause != null)
            getData.Sorts = queryParts.OrderByClause.Parameter;

        if (queryParts.Paging != null)
            getData.Page = queryParts.Paging.Parameter;


        return new CacheQueryObject
        {
            DataQueryObject = getData
        };
    }

    public static CacheQueryObject GenerateCommandQuery(this CacheQueryResult queryParts)
    {
        if (queryParts.Command == null) throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);

        return queryParts.Command.OperationType switch
        {
            DatabaseQueryOperationType.Add => queryParts.Command.GenerateAddQuery(),
            DatabaseQueryOperationType.Remove => queryParts.Command.GenerateDeleteQuery(),
            DatabaseQueryOperationType.Edit => queryParts.Command.GenerateUpdateQuery(),
            _ => throw new NotSupportedOperationException(ExceptionCode.QueryGenerator)
        };
    }

    private static CacheQueryObject GenerateAddQuery(this CacheCommandQueryPart command)
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

    private static CacheQueryObject GenerateUpdateQuery(this CacheCommandQueryPart command)
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


    private static CacheQueryObject GenerateDeleteQuery(this CacheCommandQueryPart command)
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
