using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseObjectInfo<TObject> : IObjectInfo<TObject> where TObject : class
{
    string DataSet
    {
        get;
    }

    string Schema
    {
        get;
    }

    DataSetType DataSetType
    {
        get;
    } 

}
