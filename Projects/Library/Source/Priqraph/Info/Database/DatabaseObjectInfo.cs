using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Info.Database;

public class DatabaseObjectInfo(
    string dataSet,
    DataSetType dataSetType,
    Type objectType,
    IEnumerable<IColumnPropertyInfo> propertyInfos,
    string schema = "dbo")
    : ObjectInfo<IColumnPropertyInfo>(propertyInfos, ObjectInfoType.Database, objectType), IDatabaseObjectInfo
{
    public string DataSet
    {
        get;
        init;
    } = dataSet;

    public string Schema
    {
        get;
        init;
    } = schema;

    public DataSetType DataSetType
    {
        get;
    } = dataSetType;

    public override ObjectInfoType Type => ObjectInfoType.Database;

    public override string ToString() => $"[{Schema}].[{DataSet}]";

    public static new IDatabaseObjectInfo CastObject(object value)
    {
        if (value is IDatabaseObjectInfo info)
            return info;

        try
        {
            return (IDatabaseObjectInfo)Convert.ChangeType(value, typeof(IDatabaseObjectInfo));
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException(); //todo
        }
    }
}
