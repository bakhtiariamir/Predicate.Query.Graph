using Priqraph.DataType;

namespace Priqraph.Contract;

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
