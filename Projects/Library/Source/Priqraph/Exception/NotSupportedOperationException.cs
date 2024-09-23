namespace Priqraph.Exception;

public class NotSupportedOperationException(string code) : BaseException(code)
{
    public NotSupportedOperationException(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }

    public NotSupportedOperationException(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }

    public NotSupportedOperationException(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }
}
