using Priqraph.DataType;

namespace Priqraph.Info.Database;

public static class DatabaseFunctionCollection
{
}

public class DatabaseFunction
{
    public string Name
    {
        get;
    }

    public string Schema
    {
        get;
    }

    public FunctionType FunctionType
    {
        get;
    }

    public DatabaseFunction(string name, FunctionType functionType, string schema = "dbo")
    {
        Name = name;
        FunctionType = functionType;
        Schema = schema;
    }
}
