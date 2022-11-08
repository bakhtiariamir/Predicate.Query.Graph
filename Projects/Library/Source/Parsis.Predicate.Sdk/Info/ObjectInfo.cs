using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;
public abstract class ObjectInfo<TObject> : IObjectInfo<TObject> where TObject : class
{
    public abstract IEnumerable<IPropertyInfo> PropertyInfos
    {
        get;
    }

    public abstract ObjectInfoType Type
    {
        get;
    }
}