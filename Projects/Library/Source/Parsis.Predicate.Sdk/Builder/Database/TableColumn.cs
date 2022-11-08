using System.Data;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class TableColumn
{
    public string Name
    {
        get;
        init;
    }

    public SqlDbType Type
    {
        get;
        init;
    }

    public TableColumn(string name, SqlDbType type)
    {
        Name = name;
        Type = type;
    }
}
