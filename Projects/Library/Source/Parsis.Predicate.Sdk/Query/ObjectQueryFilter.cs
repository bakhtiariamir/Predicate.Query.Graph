using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class ObjectQueryFilter<TObject> where TObject : class
{
    public Expression<Func<TObject, bool>> Predicate
    {
        get;
        set;
    }

    public ObjectQueryFilter(Expression<Func<TObject, bool>> predicate)
    {
        Predicate = predicate;
    }
}