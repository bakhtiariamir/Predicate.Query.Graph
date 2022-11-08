namespace Parsis.Predicate.Sdk.Info.Database.Attribute;
public class TableInfoAttribute : System.Attribute
{
    public string TableName
    {
        get;
        set;
    }

    public string SchemaName
    {
        get;
        set;
    }

    public TableInfoAttribute(string tableName, string schemaName = "dbo")
    {
        TableName = tableName;
        SchemaName = schemaName;
    }
}
