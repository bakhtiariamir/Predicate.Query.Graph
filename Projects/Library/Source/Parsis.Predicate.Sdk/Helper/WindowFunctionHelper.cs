using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Helper;

public static class WindowFunctionHelper
{
    public static void AddRelatedType<TMain, TRelated>(this TMain main, TRelated related) where TMain : Enum
        where TRelated : Enum => RelatedTypeCollection<TMain, TRelated>.Add(main, related);

    public static TMain FindMain<TRelated, TMain>(this TRelated related) where TMain : Enum
        where TRelated : Enum => RelatedTypeCollection<TMain, TRelated>.RelatedTypes.FirstOrDefault(item => item.Related.Equals(related))!.Main;

    public static TRelated FindRelated<TMain, TRelated>(this TMain main) where TMain : Enum
        where TRelated : Enum => RelatedTypeCollection<TMain, TRelated>.RelatedTypes.FirstOrDefault(item => item.Main.Equals(main))!.Related;

    public static IEnumerable<RelatedType<TMain, TRelated>> FindByMain<TMain, TRelated>(this TMain main) where TMain : Enum
        where TRelated : Enum => RelatedTypeCollection<TMain, TRelated>.RelatedTypes.Where(item => item.Main.Equals(main));

    public static IEnumerable<RelatedType<TMain, TRelated>> FindByRelated<TMain, TRelated>(this TRelated related) where TMain : Enum
        where TRelated : Enum => RelatedTypeCollection<TMain, TRelated>.RelatedTypes.Where(item => item.Related.Equals(related));
}
