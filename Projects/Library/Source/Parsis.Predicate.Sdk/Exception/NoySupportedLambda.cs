namespace Parsis.Predicate.Sdk.Exception;

public class NoySupportedLambda : BaseException
{
    public NoySupportedLambda(string code) : base(code)
    {
    }

    public NoySupportedLambda(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }

    public NoySupportedLambda(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }

    public NoySupportedLambda(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }

    public NoySupportedLambda(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }

    public NoySupportedLambda(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported), message, innerException)
    {
    }
}
