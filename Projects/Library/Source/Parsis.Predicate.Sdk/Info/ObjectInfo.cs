using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public abstract class ObjectInfo<TPropertyInfo> : IObjectInfo<TPropertyInfo> where TPropertyInfo : IPropertyInfo
{
    public abstract IEnumerable<TPropertyInfo> PropertyInfos
    {
        get;
    }

    public abstract ObjectInfoType Type
    {
        get;
    }

    public abstract Type ObjectType
    {
        get;
    }
}
