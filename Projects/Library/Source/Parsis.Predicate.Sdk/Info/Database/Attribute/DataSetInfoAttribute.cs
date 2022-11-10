using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database.Attribute;

[AttributeUsage(AttributeTargets.Class)]
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

    public DataSetInfoAttribute(string dataSetName, DataSetType type, string schemaName = "dbo")
    {
        DataSetName = dataSetName;
        Type = type;
        SchemaName = schemaName;
    }
}
