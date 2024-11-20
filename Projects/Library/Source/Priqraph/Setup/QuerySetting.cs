namespace Priqraph.Setup;

public class QuerySetting
{
    public IEnumerable<QueryProvider>? Providers
    {
        get;
        set;
    }

    public Database? Database
    {
        get;
        set;
    }
}