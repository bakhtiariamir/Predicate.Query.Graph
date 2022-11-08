namespace Parsis.Predicate.Sdk.Query;
public class ObjectQuery<TObject> where TObject : class
{
    public ObjectQueryFilter<TObject> Filter
    {
        get; 
        set;
    }

    public ICollection<ObjectQueryOrder<TObject>>? Orderings
    {
        get; 
        set;
    }

    public ObjectQueryPaging<TObject>? Paging
    {
        get;
        set;
    }

    public ICollection<ObjectQueryJoin<TObject>>? Joins
    {
        get;
        set;
    }

    public ObjectQuery(ObjectQueryFilter<TObject> filter)
    {
        Filter = filter;
    }



    public ObjectQuery(ObjectQueryFilter<TObject> filter, ICollection<ObjectQueryOrder<TObject>> orderings, ObjectQueryPaging<TObject> paging, ICollection<ObjectQueryJoin<TObject>> joins)
    {
        Filter = filter;
        Orderings = orderings;
        Paging = paging;
        Joins = joins;
    }
}