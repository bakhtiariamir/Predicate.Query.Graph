namespace Parsis.Predicate.Sdk.Setup;

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

public class SelectConfig
{
    public ArrayParameter NumberArray
    {
        get;
        set;
    } = ArrayParameter.PassInQuery;

    public ArrayParameter StringArray
    {
        get;
        set;
    } = ArrayParameter.PassInQuery;
}

public enum ArrayParameter
{
    PassInQuery = 1,
    PassAsParameter = 2
}

public enum QueryProvider
{
    Database,
    RestService,
    SoapService,
    Cache,
    MessageBroker
}

public class Database
{
    public QueryOptions QueryOptions
    {
        get;
        set;
    }
}

public class QueryOptions
{
    public InsertOption? InsertOption
    {
        get;
        set;
    }

    public UpdateOption? UpdateOption
    {
        get;
        set;
    }

    public DeleteOption? DeleteOption
    {
        get;
        set;
    }

    public SelectOption? SelectOption
    {
        get;
        set;
    }
}

public class SelectOption
{
    public int DefaultPageSize
    {
        get;
        set;
    } = 10;

    public LoadingRelatedObject LoadingRelatedObject
    {
        get;
        set;
    } = LoadingRelatedObject.Eager;

    public int QueryDepth
    {
        get;
        set;
    } = 2; //use Force tag to join more than Value of QueryDepth objects

    public IEnumerable<UserDefinedTable>? UserDefinedTables
    {
        get;
        set;
    }
}

public class UserDefinedTable
{
    public string Key
    {
        get;
        init;
    }

    public bool Status
    {
        get;
        set;
    }

    public string Type
    {
        get;
        set;
    }
}

public class DeleteOption
{
}

public class UpdateOption
{
}

public class InsertOption
{
}

public enum LoadingRelatedObject
{
    Explicit = 1,
    Eager = 2,
    Lazy = 3
}
