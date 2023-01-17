namespace Parsis.Predicate.Sdk.Setup;

public class QuerySetting
{
    public Global GlobalConfig
    {
        get;
        set;
    }

    public SelectConfig SelectConfig
    {
        get;
        set;
    }
}

public class Global
{
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

//public enum SqlInsertDataProvider
//{
//    Value = 1,
//    ListOfValue = 2,
//    DataTable = 3,
//    Select = 4,
//}

//public enum SqlUpdateDataProvider
//{
//    Value = 1,
//    ListOfValue = 2,
//    Select = 3
//}

//public enum LoadRelatedData
//{
//    Explicit = 1,
//    Eager = 2,
//    Lazy = 3
//}
