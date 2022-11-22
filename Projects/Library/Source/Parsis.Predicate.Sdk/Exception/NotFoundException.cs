namespace Parsis.Predicate.Sdk.Exception;
public class NotFoundException : BaseException
{
    public NotFoundException(string code) : base(code)
    {
    }
    public NotFoundException(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }
    public NotFoundException(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotFound))
    {
    }
    public NotFoundException(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound))
    {
    }
    public NotFoundException(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }
    public NotFoundException(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message, innerException)
    {
    }
}
