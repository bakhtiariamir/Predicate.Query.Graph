using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;
public class ObjectQueryJoin<TObject> where TObject : class
{
    public Expression<Func<TObject, object>> Property
    {
        get;
        set;
    }

    public Type Type
    {
        get;
        set;
    }

    public ObjectQueryJoin(Expression<Func<TObject, object>> property, Type type)
    {
        Property = property;
        Type = type;
    }
}

