namespace Parsis.Predicate.Sdk.Helper;
public static class ExceptionHelper
{
    public static string GetCodeName(this string code)
    {
        return code.Split('_')[0] switch
        {
            "1000" => QueryCreatorCodeName(code),
            "2000" => QueryObjectBuilderCodeName(code),
            "3000" => QueryBuilderCodeName(code),
            "4000" => GetQueryContectCodeName(code),
            "5000" => GetQueryCodeName(code),
            "6000" => GetObjectInfoCodeName(code),
            "7000" => GetObjectInfoAttributeCodeName(code),
            "8000" => GetQueryGeneratorCodeName(code),
            _ => "Exception_Occurred"
        };
    }

    #region 8000_Code

    private static string GetQueryGeneratorCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "0" or _ => $"{GetErrorName(code)} error occurred in query generator."
        };

    #endregion

    #region 7000_Code

    private static string GetObjectInfoAttributeCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDatabaseObjectInfoAttributeCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in attribute."
        };

    private static string GetDatabaseObjectInfoAttributeCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDataSetObjectInfoAttributeCodeName(code),
            "2" => GetColumnObjectInfoAttributeCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in database attribute."
        };

    private static string GetDataSetObjectInfoAttributeCodeName(string code) => $"{GetErrorName(code)} error occurred in dataSet attribute object.";
    private static string GetColumnObjectInfoAttributeCodeName(string code) => $"{GetErrorName(code)} error occurred in column attribute object.";

    #endregion

    #region 6000_Code

    private static string GetObjectInfoCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetCacheObjectInfoCodeName(code),
            "2" => GetDatabaseObjectInfoCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in object info."
        };

    private static string GetDatabaseObjectInfoCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDataSetObjectInfoCodeName(code),
            "2" => GetColumnObjectInfoCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in database object info."
        };

    private static string GetCacheObjectInfoCodeName(string code) => code.Split('_')[0].Split('.')[2] ?? "0" switch
    {
        "0" or _ => $"{GetErrorName(code)} error occurred in cache object info."
    };

    private static string GetDataSetObjectInfoCodeName(string code) => $"{GetErrorName(code)} error occurred in dataSet info object.";
    private static string GetColumnObjectInfoCodeName(string code) => $"{GetErrorName(code)} error occurred in column object info object.";

    #endregion

    #region 5000_Code
    private static string GetQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDatabaseQueryCodeName(code),
            "2" => GetServiceQueryCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query."
        };

    private static string GetDatabaseQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "1" => GetSqlQueryCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in database database query."
        };

    private static string GetServiceQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "0" or _ => $"{GetErrorName(code)} error occurred in service service query."
        };

    private static string GetSqlQueryCodeName(string code) => $"{GetErrorName(code)} error occurred in sql server query.";

    #endregion

    #region 4000_Code
    private static string GetQueryContectCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDatabaseQueryContextCodeName(code),
            "2" => GetServiceQueryContextCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query context builder."
        };

    private static string GetDatabaseQueryContextCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "0" or _ => $"{GetErrorName(code)} error occurred in database query context building."
        };

    private static string GetServiceQueryContextCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "0" or _ => $"{GetErrorName(code)} error occurred in service query context building."
        };

    #endregion

    #region 3000_Code

    private static string QueryBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDatabaseQueryBuilderCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query builder."
        };

    private static string GetDatabaseQueryBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "1" => GetSqlQueryBuilderCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query object info."
        };

    private static string GetSqlQueryBuilderCodeName(string code) => $"{GetErrorName(code)} error occurred during build query.";

    #endregion

    #region 2000_Code
    private static string QueryObjectBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetQueryObjectCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query object builder."
        };

    private static string GetQueryObjectCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] ?? "0" switch
        {
            "1" => GetQueryObjectSelectingCodeName(code),
            "2" => GetQueryObjectInitializingCodeName(code),
            "3" => GetQueryObjectFilteringCodeName(code),
            "4" => GetQueryObjectSortingCodeName(code),
            "5" => GetQueryObjectPagingCodeName(code),
            "6" => GetQueryObjectJoiningCodeName(code),
            "7" => GetQueryObjectGroupingCodeName(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query object info."
        };

    private static string GetQueryObjectSelectingCodeName(string code) => $"{GetErrorName(code)} error occurred during manage fields for query.";

    private static string GetQueryObjectInitializingCodeName(string code) => $"{GetErrorName(code)} error occurred during initialize fields fields for query.";

    private static string GetQueryObjectFilteringCodeName(string code) => $"{GetErrorName(code)} error occurred during filters fields for query.";

    private static string GetQueryObjectSortingCodeName(string code) => $"{GetErrorName(code)} error occurred during manage sorts for query.";

    private static string GetQueryObjectPagingCodeName(string code) => $"{GetErrorName(code)} error occurred during manage paging for query.";

    private static string GetQueryObjectJoiningCodeName(string code) => $"{GetErrorName(code)} error occurred during manage join objects for query.";

    private static string GetQueryObjectGroupingCodeName(string code) => $"{GetErrorName(code)} error occurred during manage group objects for query";

    #endregion

    #region 1000_Code

    private static string QueryCreatorCodeName(string code)
    {
        return code.Split('_')[0].Split('.')[1] ?? "0" switch
        {
            "1" => GetDatabaseQueryCreator(code),
            "2" => GetServiceQueryCreator(code),
            "0" or _ => $"{GetErrorName(code)} error occurred in query creator."
        };
    }

    private static string GetDatabaseQueryCreator(string code) => code.Split('_')[0].Split('.')[2] ?? "0" switch
    {
        "1" => GetSqlServerQueryCreator(code),
        "0" or _ => $"{GetErrorName(code)} error occurred during in server query creator."
    };


    private static string GetServiceQueryCreator(string code) => code.Split('_')[0].Split('.')[2] ?? "0" switch
    {
        "1" => GetApiQueryCreator(code),
        "0" or _ => $"{GetErrorName(code)} error occurred during in query creator."
    };

    private static string GetSqlServerQueryCreator(string code) => $"{GetErrorName(code)} error occurred during sql server query creation.";
    private static string GetApiQueryCreator(string code) => $"{GetErrorName(code)} error occurred during api query creation.";

    #endregion

    #region Public_Code_Name_Method
    private static object GetErrorName(string code) =>
        GetEntityName(code) + code.Split('@')[1] ?? "UnKnown" switch
        {
            "ArgumentException" => "_Invalid_Argument_Provided",
            "ArgumentNullException" => "_No_Argument_Provided",
            "ArgumentOutOfRangeException" => $"_Argument_Not_In_Range",
            "NullReferenceException" => $"_Invalid_Null_Reference",
            "IndexOutOfRangeException" => $"_Index_Array_Out_Of_Range",
            "NotSupportedException" => "_Not_Supported",
            "FormatException" => "_Argument_Format_Invalid",
            "RankException" => $"Array_With_Wrong_Number",
            "TimeoutException" => $"_TimeOut",
            "UriFormatException" => $"_Invalid_Uri_Format",
            "FileFormatException" => $"Invalid_File_Format",
            "FileNotFoundException" => $"File_Not_Found",
            "OverflowException" => $"_Operation_Overflow",
            "InvalidOperationException" => $"_Invalid_Operation",
            "InvalidCastException" => $"_Invalid_Cast",
            "OutOfMemoryException" => $"_Operation_Out_Of_Memory",
            "UnKnown" or _ => "_UnKnown_Error"
        };

    private static object GetEntityName(string code) => code.Split('_')[1].Split('@')[0] ?? "Unkown";

    #endregion
}
