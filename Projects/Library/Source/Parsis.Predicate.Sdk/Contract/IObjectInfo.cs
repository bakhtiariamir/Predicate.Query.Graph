using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IObjectInfo<TObject> where TObject : class
{

    IEnumerable<IPropertyInfo> PropertyInfos
    {
        get;
    }
    ObjectInfoType Type
    {
        get;
    }
}
