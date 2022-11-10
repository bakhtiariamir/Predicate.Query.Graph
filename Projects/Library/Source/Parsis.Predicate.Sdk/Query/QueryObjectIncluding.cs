namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectIncluding<TObject> where TObject : class
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
