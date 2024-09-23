namespace Priqraph.Exception;

public class NotSupportedLambdaException : BaseException
{
    public NotSupportedLambdaException(string code) : base(code)
    {
    }

    public NotSupportedLambdaException(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }

    public NotSupportedLambdaException(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }

    public NotSupportedLambdaException(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }

    public NotSupportedLambdaException(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }

    public NotSupportedLambdaException(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported), message, innerException)
    {
    }
}
