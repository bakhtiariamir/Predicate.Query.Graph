using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;

public interface IObjectInfo<out TPropertyInfo> : IObjectInfo where TPropertyInfo : IPropertyInfo
{
    IEnumerable<TPropertyInfo> PropertyInfos
    {
        get;
    }
}


public interface IObjectInfo
{
    //IEnumerable<object> PropertyInfos
    //{
    //    get;
    //}

    ObjectInfoType Type
    {
        get;
    }

    Type ObjectType
    {
        get;
    }
}