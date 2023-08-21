﻿using Priqraph.Contract;
using Priqraph.DataType;
using System.Linq.Expressions;

namespace Priqraph.Query;

public class QueryObjectSorting<TObject> : IQueryObjectPart<QueryObjectSorting<TObject>, ICollection<SortPredicate<TObject>>> where TObject : IQueryableObject
{
    private ICollection<SortPredicate<TObject>> _orderPredicates;

    private QueryObjectSorting() => _orderPredicates = new List<SortPredicate<TObject>>();

    public static QueryObjectSorting<TObject> Init() => new();

    public QueryObjectSorting<TObject> Add(Expression<Func<TObject, object>> expression, DirectionType direction)
    {
        _orderPredicates.Add(new SortPredicate<TObject>(expression, direction));
        return this;
    }

    public bool HasOrder() => _orderPredicates.Count > 0;

    public ICollection<SortPredicate<TObject>> Return() => _orderPredicates;

    public Dictionary<string, string> GetQueryOptions() => new();

    public QueryObjectSorting<TObject> Validate() => this;
}

public class SortPredicate<TObject> where TObject : IQueryableObject
{
    public Expression<Func<TObject, object>>? Expression
    {
        get;
    }

    public DirectionType DirectionType
    {
        get;
    }

    internal SortPredicate(Expression<Func<TObject, object>> expression, DirectionType directionType)
    {
        Expression = expression;
        DirectionType = directionType;
    }

    public static SortPredicate<TObject> CreateSorPredicate(Expression<Func<TObject, object>> expression, DirectionType directionType) => new (expression, directionType);
}