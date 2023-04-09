﻿using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Generator.Cache;

public class CacheOrdersByClauseQueryPart : CacheQueryPart<ICollection<CacheSortPredicate>>
{
    private CacheOrdersByClauseQueryPart(CacheSortPredicate cacheSortPredicate)
    {
        Parameter.Add(cacheSortPredicate);
    }

    private CacheOrdersByClauseQueryPart(CacheSortPredicate[] columnSortPredicates)
    {
        Parameter = columnSortPredicates;
    }

    public static CacheOrdersByClauseQueryPart Create(params CacheSortPredicate[] columnSortPredicates) => new(columnSortPredicates);

    public static CacheOrdersByClauseQueryPart Create(CacheSortPredicate cacheSortPredicate) => new(cacheSortPredicate);

    public static CacheOrdersByClauseQueryPart Merged(IEnumerable<CacheOrdersByClauseQueryPart> orderByClauses) => new CacheOrdersByClauseQueryPart(orderByClauses.SelectMany(item => item.Parameter).ToArray());

    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private string SetDirection(DirectionType directionType) => directionType switch {
        DirectionType.Asc => "asc",
        DirectionType.Desc => "desc",
        _ => throw new NotSupported("DirectionType", directionType.ToString(), ExceptionCode.DatabaseQuerySortingGenerator)
    };
}

public class CacheSortPredicate
{
    public IPropertyInfo PropertyInfo
    {
        get;
    }

    public DirectionType DirectionType
    {
        get;
    }

    public CacheSortPredicate(IPropertyInfo propertyInfo, DirectionType directionType = DirectionType.Asc)
    {
        PropertyInfo = propertyInfo;
        DirectionType = directionType;
    }
}
