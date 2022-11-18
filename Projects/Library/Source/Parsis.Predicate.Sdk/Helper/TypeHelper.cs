using Parsis.Predicate.Sdk.Info.Database.Attribute;

namespace Parsis.Predicate.Sdk.Helper;
public static class TypeHelper
{
    public static bool IsDataSet(this Type type) => type.GetClassAttribute<DataSetInfoAttribute>() != null;
}
