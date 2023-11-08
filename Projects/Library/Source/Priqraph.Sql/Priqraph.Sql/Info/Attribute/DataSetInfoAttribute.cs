using Priqraph.DataType;

namespace Priqraph.Sql.Info.Attribute;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class DataSetInfoAttribute : System.Attribute
{
    public string DataSetName
    {
        get;
    }

    public string SchemaName
    {
        get;
    }

    public DataSetType Type
    {
        get;
    }

    public DataSetInfoAttribute(string dataSetName, DataSetType type = DataSetType.Table, string schemaName = "dbo")
    {
        DataSetName = dataSetName;
        Type = type;
        SchemaName = schemaName;
    }
}
