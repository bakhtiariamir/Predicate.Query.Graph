using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class ObjectQueryOrder<TObject> where TObject : class
{
    public Expression<Func<TObject, object>> Property
    {
        get;
        set;
    }

    public OrderDirection Direction
    {
        get;
        set;
    }

    public ObjectQueryOrder(Expression<Func<TObject, object>> property, OrderDirection direction)
    {
        Property = property;
        Direction = direction;
    }
}

public enum OrderDirection
{
    Asc = 1,
    Desc = 2,
}