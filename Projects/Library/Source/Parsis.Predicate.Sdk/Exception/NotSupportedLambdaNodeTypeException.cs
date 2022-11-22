namespace Parsis.Predicate.Sdk.Exception;
public class NotSupportedLambdaNodeTypeException : BaseException
{
    public NotSupportedLambdaNodeTypeException(string code) : base(code)
    {
    }
    public NotSupportedLambdaNodeTypeException(string code, string? message, System.Exception? innerException) : base(code, message, innerException)
    {
    }
    public NotSupportedLambdaNodeTypeException(string objectName, string code) : this(string.Format(code, $"{objectName}", ErrorCode.NotSupported))
    {
    }
    public NotSupportedLambdaNodeTypeException(string objectName, string propertyName, string code) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported))
    {
    }
    public NotSupportedLambdaNodeTypeException(string objectName, string propertyName, string code, string? message) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotFound), message)
    {
    }
    public NotSupportedLambdaNodeTypeException(string objectName, string propertyName, string code, string? message, System.Exception? innerException) : this(string.Format(code, $"{objectName}.{propertyName}", ErrorCode.NotSupported), message, innerException)
    {
    }
}