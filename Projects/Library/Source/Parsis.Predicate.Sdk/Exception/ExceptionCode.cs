namespace Parsis.Predicate.Sdk.Exception;
public static class ExceptionCode
{
    /// <summary>
    /// Query creator object exception based on objects
    /// </summary>
    public static string QueryCreator = "1000_{object_name}@{error_code}";
    public static string DatabaseQueryCreator = "1000.1_{object_name}@{error_code}";
    public static string SqlServerQueryCreator = "1000.1.1_{object_name}@{error_code}";
    public static string ServiceQueryCreator = "1000.2_{object_name}@{error_code}";
    public static string ApiQueryCreator = "1000.2.1_{object_name}@{error_code}";
    /// <summary>
    /// Query object exception based on objects
    /// </summary>
    public static string QueryObjectBuilder = "2000_{object_name}@{error_code}";
    public static string QueryObject = "2000.1_{object_name}@{error_code}";
    public static string QueryObjectSelecting = "2000.1.1_{object_name}@{error_code}";
    public static string QueryObjectInitializing = "2000.1.2_{object_name}@{error_code}";
    public static string QueryObjectFiltering = "2000.1.3_{object_name}@{error_code}";
    public static string QueryObjectSorting = "2000.1.4_{object_name}@{error_code}";
    public static string QueryObjectPaging = "2000.1.5_{object_name}@{error_code}";
    public static string QueryObjectJoining = "2000.1.6_{object_name}@{error_code}";
    public static string QueryObjectGrouping = "2000.1.7_{object_name}@{error_code}";
    /// <summary>
    /// Query builder object exception based on objects
    /// </summary>
    public static string QueryBuilder = "3000_{object_name}@{error_code}";
    public static string DatabaseQueryBuilder = "3000.1_{object_name}@{error_code}";
    public static string SqlServerQueryBuilder = "3000.1.1_{object_name}@{error_code}";
    public static string ServiceQueryBuilder = "3000.2_{object_name}@{error_code}";
    public static string ApiQueryBuilder = "3000.2.1_{object_name}@{error_code}";
    /// <summary>
    /// Query context builder object exception based on objects
    /// </summary>
    public static string QueryContextBuilder = "4000_{object_name}@{error_code}";
    public static string DatabaseQueryContextBuilder = "4000.1_{object_name}@{error_code}";
    public static string ServiceQueryContextBuilder = "4000.2_{object_name}@{error_code}";
    /// <summary>
    /// Query exception based on objects
    /// </summary>
    public static string Query = "5000_{object_name}@{error_code}";
    public static string DatabaseQuery = "5000.1_{object_name}@{error_code}";
    public static string SqlServerQuery = "5000.1.1_{object_name}@{error_code}";
    public static string ServiceQuery = "5000.2_{object_name}@{error_code}";
    /// <summary>
    /// Object info exception based on objects
    /// </summary>
    public static string ObjectInfo = "6000_{object_name}@{error_code}";
    public static string CachedObjectInfo = "6000.1_{object_name}@{error_code}";
    public static string DatabaseObjectInfo = "6000.2_{object_name}@{error_code}";
    public static string DataSetObjectInfo = "6000.2.1_{object_name}@{error_code}";
    public static string ColumnObjectInfo = "6000.2.2_{object_name}@{error_code}";
    /// <summary>
    /// Object info attributes exception based on objects
    /// </summary>
    public static string ObjectInfoAttribute = "7000_{object_name}@{error_code}";
    public static string DatabaseInfoAttribute = "7000.1_{object_name}@{error_code}";
    public static string DataSetInfoAttribute = "7000.1.1_{object_name}@{error_code}";
    public static string ColumnInfoAttribute = "7000.1.2_{object_name}@{error_code}";
    /// <summary>
    /// Query generator 
    /// </summary>
    public static string QueryGenerator = "8000_{object_name}@{error_code}";
    public static string DatabaseQueryGenerator = "8000.1_{object_name}@{error_code}";
    public static string DatabaseQueryGeneratorGetProperty = "8000.1.1_{object_name}@{error_code}";
    public static string DatabaseQuerySelectingGenerator = "8000.1.2_{object_name}@{error_code}";
    public static string DatabaseQueryFilteringGenerator = "8000.1.3_{object_name}@{error_code}";
    public static string DatabaseQuerySortingGenerator = "8000.1.4_{object_name}@{error_code}";
    public static string DatabaseQueryPagingGenerator = "8000.1.5_{object_name}@{error_code}";
    public static string DatabaseQueryJoiningGenerator = "8000.1.6_{object_name}@{error_code}";
    public static string DatabaseQueryGroupByGenerator = "8000.1.7_{object_name}@{error_code}";
    public static string ApiQueryGenerator = "8000.2_{object_name}@{error_code}";
}


public enum ErrorCode
{
    NotFound = 1,
    BadRequest = 2,
    NotSupported = 3
}
