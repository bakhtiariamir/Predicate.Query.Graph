using Priqraph.Helper;

namespace Priqraph.Exception;

public abstract class BaseException : System.Exception
{
    private string CodeName
    {
        get;
    }

    protected BaseException(string code)
    {
        CodeName = code.GetCodeName();
    }

    protected BaseException(string code, string? message) : base(message)
    {
        CodeName = code.GetCodeName();
    }

    protected BaseException(string code, string? message, System.Exception? innerException) : base(message, innerException)
    {
        CodeName = code.GetCodeName();
    }
}
