namespace Parsis.Predicate.Sdk.Query;

public class PageSetting<TObject> where TObject : class
{
    public long Size
    {
        get;
    }

    public long Count
    {
        get;
    }

    public PageSetting(long size, long count)
    {
        Size = size;
        Count = count;
    }

}