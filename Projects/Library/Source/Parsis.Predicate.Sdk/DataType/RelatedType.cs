namespace Parsis.Predicate.Sdk.DataType;
public class RelatedType<TMain, TRelated> where TMain : Enum where TRelated : Enum
{
    public TMain Main
    {
        get;
    }

    public TRelated Related
    {
        get;
    }

    public RelatedType(TMain main, TRelated related)
    {
        Main = main;
        Related = related;
    }
}
