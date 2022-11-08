namespace Parsis.Predicate.Sdk.Exception;
public class NotSupportedLambdaNodeType : BaseException
{
    public NotSupportedLambdaNodeType() : base(ExceptionCode.QueryGeneratorLambdaNodeTypeNotSupported)
    {
    }

    public NotSupportedLambdaNodeType(string message) : base(ExceptionCode.QueryGeneratorLambdaNodeTypeNotSupported, message)
    {
    }
}
