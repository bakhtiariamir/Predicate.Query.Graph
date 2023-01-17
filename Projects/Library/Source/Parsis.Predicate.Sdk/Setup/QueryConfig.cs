namespace Parsis.Predicate.Sdk.Setup;

public class QueryConfig
{
    public class SqlQueryConfig
    {
        public int DefaultPageRows
        {
            get;
            set;
        } = 10;
    }

    public class ApiQueryConfig
    {
    }
}

public enum SqlInsertDataProvider
{
    Value = 1,
    ListOfValue = 2,
    DataTable = 3,
    Select = 4,
}

public enum SqlUpdateDataProvider
{
    Value = 1,
    ListOfValue = 2,
    Select = 3
}

public enum LoadRelatedData
{
    Explicit = 1,
    Eager = 2,
    Lazy = 3
}
