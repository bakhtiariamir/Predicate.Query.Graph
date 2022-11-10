using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectObjectSorting<TObject> : IQueryObjectPart<QueryObjectObjectSorting<TObject>> where TObject : class
{
    private readonly IList<SortPredicate<TObject>> _orderPredicates;
    private QueryObjectObjectSorting()
    {
        _orderPredicates = new List<SortPredicate<TObject>>();
    }

    public static QueryObjectObjectSorting<TObject> Init() => new();

    public QueryObjectObjectSorting<TObject> Add(Expression<Func<TObject, object>> expression, DirectionType direction)
    {
        _orderPredicates.Add(new SortPredicate<TObject>(expression, direction));
        return this;
    }

    public IList<SortPredicate<TObject>> Return() => _orderPredicates;


    public QueryObjectObjectSorting<TObject> Validation() => this;
}

public class SortPredicate<TObject> where TObject : class
{
    public Expression<Func<TObject, object>> Expression
    {
        get;
    }

    public DirectionType DirectionType
    {
        get;
    }

    public SortPredicate(Expression<Func<TObject, object>> expression, DirectionType directionType)
    {
        Expression = expression;
        DirectionType = directionType;
    }
}

