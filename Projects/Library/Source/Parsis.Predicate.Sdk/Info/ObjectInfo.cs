using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public class ObjectInfo<TPropertyInfo> : IObjectInfo<TPropertyInfo> where TPropertyInfo : IPropertyInfo
{
    public ObjectInfo(IEnumerable<TPropertyInfo> propertyInfos, ObjectInfoType type, Type objectType)
    {
        PropertyInfos = propertyInfos;
        Type = type;
        ObjectType = objectType;
    }

    public virtual IEnumerable<TPropertyInfo> PropertyInfos
    {
        get;
    }

    public virtual ObjectInfoType Type
    {
        get;
    }

    public virtual Type ObjectType
    {
        get;
    }

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
