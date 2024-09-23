using Priqraph.DataType;

namespace Priqraph.Info.Database.Attribute;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class DataSetInfoAttribute(string dataSetName, DataSetType type = DataSetType.Table, string schemaName = "dbo")
    : System.Attribute
{
    public string DataSetName
    {
        get;
    } = dataSetName;

    public string SchemaName
    {
        get;
    } = schemaName;

    public DataSetType Type
    {
        get;
    } = type;
}
