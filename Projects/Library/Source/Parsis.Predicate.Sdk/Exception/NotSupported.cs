namespace Parsis.Predicate.Sdk.Exception;
public class NotSupported : BaseException
{
    public NotSupported(string code) : base(code)
    {
    }
    public NotSupported(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }
    public NotSupported(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }
    public NotSupported(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }
    public NotSupported(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }
    public NotSupported(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported), message, innerException)
    {
    }
}