using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IObjectInfo<out TPropertyInfo> where TPropertyInfo : IPropertyInfo
{

    IEnumerable<TPropertyInfo> PropertyInfos
    {
        get;
    }
    ObjectInfoType Type
    {
        get;
    }


    Type ObjectType
    {
        get;
    }
}
