using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Contract;

public interface IDatabaseObjectInfo : IObjectInfo<IColumnPropertyInfo>
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
