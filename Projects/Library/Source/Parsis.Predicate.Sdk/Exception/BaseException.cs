using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Exception;
public class BaseException : System.Exception
{
    public string Code
    {
        get;
        private set;
    }

    public string CodeName
    {
        get;
    }

    public BaseException(string code)
    {
        Code = code;
        CodeName = code.GetCodeName();
    }

    public BaseException(string code, string? message) : base(message)
    {
        Code = code;
        CodeName = code.GetCodeName();
    }

    public BaseException(string code, string? message, System.Exception? innerException) : base(message, innerException)
    {
        Code = code;
        CodeName = code.GetCodeName();
    }
}
