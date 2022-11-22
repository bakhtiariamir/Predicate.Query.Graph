namespace Parsis.Predicate.Sdk.Exception;
public class NotSupportedException : BaseException
{
    public NotSupportedException(string code) : base(code)
    {
    }
    public NotSupportedException(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }
    public NotSupportedException(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }
    public NotSupportedException(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }
    public NotSupportedException(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }
    public NotSupportedException(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported), message, innerException)
    {
    }
}