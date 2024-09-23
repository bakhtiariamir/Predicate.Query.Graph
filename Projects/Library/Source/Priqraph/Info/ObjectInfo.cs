using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Info;

public class ObjectInfo<TPropertyInfo>(IEnumerable<TPropertyInfo> propertyInfos, ObjectInfoType type, Type objectType)
    : IObjectInfo<TPropertyInfo>
    where TPropertyInfo : IPropertyInfo
{
    public virtual IEnumerable<TPropertyInfo> PropertyInfos
    {
        get;
    } = propertyInfos;

    public virtual ObjectInfoType Type
    {
        get;
    } = type;

    public virtual Type ObjectType
    {
        get;
    } = objectType;

    public static IObjectInfo<IPropertyInfo> CastObject(object value)
    {
        if (value is IObjectInfo<IPropertyInfo> info)
            return info;

        try
        {
            return (ObjectInfo<IPropertyInfo>)Convert.ChangeType(value, typeof(IObjectInfo<IPropertyInfo>));
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException(); //todo
        }
    }
}
