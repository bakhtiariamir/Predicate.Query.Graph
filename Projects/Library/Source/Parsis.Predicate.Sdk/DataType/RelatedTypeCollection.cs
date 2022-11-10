using System.ComponentModel;

namespace Parsis.Predicate.Sdk.DataType;
public static class RelatedTypeCollection<TMain, TRelated> where TMain : Enum where TRelated : Enum
{
    public static IList<RelatedType<TMain, TRelated>> RelatedTypes
    {
        get;
    } = new List<RelatedType<TMain, TRelated>>();

    public static void Add(TMain main, TRelated related)
    {
        var findItems = RelatedTypes.FirstOrDefault(item => item.Main.Equals(main) || item.Related.Equals(related));
        if (findItems != null) RelatedTypes.Add(new RelatedType<TMain, TRelated>(main, related));
    }
}
