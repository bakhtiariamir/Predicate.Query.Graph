namespace Priqraph.Exception;

public static class ExceptionCode
{
    /// <summary>
    /// Query creator object exception based on objects
    /// </summary>
    public static readonly string QueryCreator = "1000.0_{0}@{1}";
    public  static readonly string DatabaseQueryCreator = "1000.1_{0}@{1}";
    public  static readonly string SqlServerQueryCreator = "1000.1.1_{0}@{1}";
    public  static readonly string ServiceQueryCreator = "1000.2_{0}@{1}";
    public  static readonly string ApiQueryCreator = "1000.2.1_{0}@{1}";

    /// <summary>
    /// Query object exception based on objects
    /// </summary>
    public  static readonly string QueryObjectBuilder = "2000.0_{0}@{1}";
    public  static readonly string QueryObject = "2000.1_{0}@{1}";
    public  static readonly string QueryObjectSelecting = "2000.1.1_{0}@{1}";
    public  static readonly string QueryObjectInitializing = "2000.1.2_{0}@{1}";
    public  static readonly string QueryObjectFiltering = "2000.1.3_{0}@{1}";
    public  static readonly string QueryObjectSorting = "2000.1.4_{0}@{1}";
    public  static readonly string QueryObjectPaging = "2000.1.5_{0}@{1}";
    public  static readonly string QueryObjectJoining = "2000.1.6_{0}@{1}";
    public  static readonly string QueryObjectGrouping = "2000.1.7_{0}@{1}";

    /// <summary>
    /// Query builder object exception based on objects
    /// </summary>
    public  static readonly string QueryBuilder = "3000.0_{0}@{1}";
    public  static readonly string DatabaseQueryBuilder = "3000.1_{0}@{1}";
    public  static readonly string SqlServerQueryBuilder = "3000.1.1_{0}@{1}";
    public  static readonly string ServiceQueryBuilder = "3000.2_{0}@{1}";
    public  static readonly string ApiQueryBuilder = "3000.2.1_{0}@{1}";

    /// <summary>
    /// Query context builder object exception based on objects
    /// </summary>
    public  static readonly string QueryContextBuilder = "4000.0_{0}@{1}";
    public  static readonly string DatabaseQueryContextBuilder = "4000.1_{0}@{1}";
    public  static readonly string ServiceQueryContextBuilder = "4000.2_{0}@{1}";

    /// <summary>
    /// Query exception based on objects
    /// </summary>
    public  static readonly string Query = "5000.0_{0}@{1}";
    public  static readonly string DatabaseQuery = "5000.1_{0}@{1}";
    public  static readonly string SqlServerQuery = "5000.1.1_{0}@{1}";
    public  static readonly string ServiceQuery = "5000.2_{0}@{1}";

    /// <summary>
    /// Object info exception based on objects
    /// </summary>
    public  static readonly string ObjectInfo = "6000.0_{0}@{1}";
    public  static readonly string CachedObjectInfo = "6000.1_{0}@{1}";
    public  static readonly string DatabaseObjectInfo = "6000.2_{0}@{1}";
    public  static readonly string DataSetObjectInfo = "6000.2.1_{0}@{1}";
    public  static readonly string ColumnObjectInfo = "6000.2.2_{0}@{1}";

    /// <summary>
    /// Object info attributes exception based on objects
    /// </summary>
    public  static readonly string ObjectInfoAttribute = "7000.0_{0}@{1}";
    public  static readonly string DatabaseInfoAttribute = "7000.1_{0}@{1}";
    public  static readonly string DataSetInfoAttribute = "7000.1.1_{0}@{1}";
    public  static readonly string ColumnInfoAttribute = "7000.1.2_{0}@{1}";

    /// <summary>
    /// Query generator 
    /// </summary>
    public  static readonly string QueryGenerator = "8000.0_{0}@{1}";
    public  static readonly string DatabaseQueryGenerator = "8000.1_{0}@{1}";
    public  static readonly string DatabaseQueryGeneratorGetProperty = "8000.1.1_{0}@{1}";
    public  static readonly string DatabaseQuerySelectingGenerator = "8000.1.2_{0}@{1}";
    public  static readonly string DatabaseQueryFilteringGenerator = "8000.1.3_{0}@{1}";
    public  static readonly string DatabaseQuerySortingGenerator = "8000.1.4_{0}@{1}";
    public  static readonly string DatabaseQueryPagingGenerator = "8000.1.5_{0}@{1}";
    public  static readonly string DatabaseQueryJoiningGenerator = "8000.1.6_{0}@{1}";
    public  static readonly string DatabaseQueryGroupByGenerator = "8000.1.7_{0}@{1}";
    public  static readonly string ApiQueryGenerator = "8000.2_{0}@{1}";
}