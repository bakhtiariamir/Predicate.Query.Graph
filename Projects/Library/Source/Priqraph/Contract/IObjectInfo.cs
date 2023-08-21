using Priqraph.DataType;

namespace Priqraph.Contract;

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
