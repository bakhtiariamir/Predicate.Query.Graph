namespace Parsis.Predicate.Sdk.Query;

public class ObjectQueryPaging<TObject> where TObject : class
{
    public long Size
    {
        get;
        set;
    }

    public long Count
    {
        get;
        set;
    }

    public ObjectQueryPaging(long size, long count)
    {
        Size = size;
        Count = count;
    }

}