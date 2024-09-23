namespace Priqraph.Helper;

internal static class ExceptionHelper
{
    public static string GetCodeName(this string code)
    {
        return code.Split('_')[0].Split('.')[0] switch {
            "1000"  => QueryCreatorCodeName(code),
            "2000"  => QueryObjectBuilderCodeName(code),
            "3000"  => QueryBuilderCodeName(code),
            "4000"  => GetQueryConnectCodeName(code),
            "5000"  => GetQueryCodeName(code),
            "6000"  => GetObjectInfoCodeName(code),
            "7000"  => GetObjectInfoAttributeCodeName(code),
            "8000"  => GetQueryGeneratorCodeName(code),
            _ => "Exception_Occurred"
        };
    }

    #region 8000_Code

    private static string GetQueryGeneratorCodeName(string code) =>
        code.Split('_')[0].Split('.')[1]  switch {
            "0" or _ => $"{GetErrorName(code)}"
        };

    #endregion

    #region 7000_Code

    private static string GetObjectInfoAttributeCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDatabaseObjectInfoAttributeCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDatabaseObjectInfoAttributeCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDataSetObjectInfoAttributeCodeName(code),
            "2" => GetColumnObjectInfoAttributeCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDataSetObjectInfoAttributeCodeName(string code) => $"{GetErrorName(code)} error occurred in dataSet attribute object.";

    private static string GetColumnObjectInfoAttributeCodeName(string code) => $"{GetErrorName(code)} error occurred in column attribute object.";

    #endregion

    #region 6000_Code

    private static string GetObjectInfoCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetCacheObjectInfoCodeName(code),
            "2" => GetDatabaseObjectInfoCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDatabaseObjectInfoCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDataSetObjectInfoCodeName(code),
            "2" => GetColumnObjectInfoCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetCacheObjectInfoCodeName(string code) => code.Split('_')[0].Split('.')[2] switch {
        "0" or _ => $"{GetErrorName(code)}"
    };

    private static string GetDataSetObjectInfoCodeName(string code) => $"{GetErrorName(code)} error occurred in dataSet info object.";

    private static string GetColumnObjectInfoCodeName(string code) => $"{GetErrorName(code)} error occurred in column object info object.";

    #endregion

    #region 5000_Code

    private static string GetQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDatabaseQueryCodeName(code),
            "2" => GetServiceQueryCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDatabaseQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "1" => GetSqlQueryCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetServiceQueryCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetSqlQueryCodeName(string code) => $"{GetErrorName(code)}";

    #endregion

    #region 4000_Code

    private static string GetQueryConnectCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDatabaseQueryContextCodeName(code),
            "2" => GetServiceQueryContextCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDatabaseQueryContextCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetServiceQueryContextCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "0" or _ => $"{GetErrorName(code)}"
        };

    #endregion

    #region 3000_Code

    private static string QueryBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDatabaseQueryBuilderCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetDatabaseQueryBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "1" => GetSqlQueryBuilderCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetSqlQueryBuilderCodeName(string code) => $"{GetErrorName(code)}";

    #endregion

    #region 2000_Code

    private static string QueryObjectBuilderCodeName(string code) =>
        code.Split('_')[0].Split('.')[1] switch {
            "1" => GetQueryObjectCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetQueryObjectCodeName(string code) =>
        code.Split('_')[0].Split('.')[2] switch {
            "1" => GetQueryObjectSelectingCodeName(code),
            "2" => GetQueryObjectInitializingCodeName(code),
            "3" => GetQueryObjectFilteringCodeName(code),
            "4" => GetQueryObjectSortingCodeName(code),
            "5" => GetQueryObjectPagingCodeName(code),
            "6" => GetQueryObjectJoiningCodeName(code),
            "7" => GetQueryObjectGroupingCodeName(code),
            "0" or _ => $"{GetErrorName(code)}"
        };

    private static string GetQueryObjectSelectingCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectInitializingCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectFilteringCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectSortingCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectPagingCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectJoiningCodeName(string code) => $"{GetErrorName(code)}";

    private static string GetQueryObjectGroupingCodeName(string code) => $"{GetErrorName(code)}";

    #endregion

    #region 1000_Code

    private static string QueryCreatorCodeName(string code)
    {
        return code.Split('_')[0].Split('.')[1] switch {
            "1" => GetDatabaseQueryCreator(code),
            "2" => GetServiceQueryCreator(code),
            "0" or _ => $"{GetErrorName(code)}"
        };
    }

    private static string GetDatabaseQueryCreator(string code) => code.Split('_')[0].Split('.')[2] switch {
        "1" => GetSqlServerQueryCreator(code),
        "0" or _ => $"{GetErrorName(code)}"
    };

    private static string GetServiceQueryCreator(string code) => code.Split('_')[0].Split('.')[2] switch {
        "1" => GetApiQueryCreator(code),
        "0" or _ => $"{GetErrorName(code)}"
    };

    private static string GetSqlServerQueryCreator(string code) => $"{GetErrorName(code)}";

    private static string GetApiQueryCreator(string code) => $"{GetErrorName(code)}";

    #endregion

    #region Public_Code_Name_Method

    private static object GetErrorName(string code) =>
        GetEntityName(code) + code.Split('@')[1] switch {
            "IsNull" => "_No_Argument_Provided",
            "NotValid" => "_Not_Valid",
            "NotSupported" => "_Not_Supported",
            "InvalidCast" => $"_Invalid_Cast",
            "InvalidFormat" => "_Argument_Format_Invalid",
            "InvalidArgument" => "_Invalid_Argument_Provided",
            
            "ArgumentOutOfRangeException" => $"_Argument_Not_In_Range",
            "NullReferenceException" => $"_Invalid_Null_Reference",
            "IndexOutOfRangeException" => $"_Index_Array_Out_Of_Range",
            "FormatException" => "_Argument_Format_Invalid",
            "RankException" => $"Array_With_Wrong_Number",
            "OverflowException" => $"_Operation_Overflow",

            "OutOfMemoryException" => $"_Operation_Out_Of_Memory",
            "UnKnown" or _ => "_UnKnown_Error"
        };

    private static object GetEntityName(string code) => code.Split('_')[1].Split('@')[0];

    #endregion
}
