namespace Parsis.Predicate.Sdk.Helper;
public static class ExceptionHelper
{
    public static string GetCodeName(this string code)
    {
        return code.Split('.')[0] switch
        {
            "1000" => GetNotSupportedCodeName(code),
            _ => "Exception_Occurred"
        };
    }

    private static string GetNotSupportedCodeName(string code)
    {
        return code.Split('.')[1] switch
        {
            "0" => GetGlobalQueryGeneratorCodeName(code),
            "1" => GetDatabaseQueryGeneratorCodeName(code),
            _ => "Not_Supported",
        };
    }

    private static string GetDatabaseQueryGeneratorCodeName(string code)
    {
        return code.Split('.')[2] switch
        {
            "1" => "Lamda_Expression_Node_Type_Not_Supported",
            "0" or _ => "Entity_Not_Found"
        };
    }

    private static string GetGlobalQueryGeneratorCodeName(string code)
    {
        return code.Split('.')[2] switch
        {
            "0" or _ => "Configuration_Not_Found"
        };
    }
}
