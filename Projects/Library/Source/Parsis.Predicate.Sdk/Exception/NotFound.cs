using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.Exception;

public class NotFound : BaseException
{
    public NotFound(string code) : base(code)
    {
    }

    public NotFound(string objectName, string code) : base(code)
    {
    }

    private NotFound(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }

    public NotFound(string objectName, PartType parameterPartType, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotFound))
    {
    }

    public NotFound(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound))
    {
    }

    public NotFound(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound))
    {
    }

    public NotFound(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message, innerException)
    {
    }
}
