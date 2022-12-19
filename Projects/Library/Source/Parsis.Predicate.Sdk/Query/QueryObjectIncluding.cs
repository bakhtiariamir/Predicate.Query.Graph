using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectIncluding<TObject> where TObject : IQueryableObject
{
    public QueryObjectIncluding(Func<TObject, object> property)
    {
        Property = property;
    }

    public Func<TObject, object> Property
    {
        get;
        set;
    }

}
