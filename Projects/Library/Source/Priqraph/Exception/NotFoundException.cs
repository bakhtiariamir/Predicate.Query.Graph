using Priqraph.DataType;
using Priqraph.Generator.Database;

namespace Priqraph.Exception;

public class NotFoundException : BaseException
{
    public NotFoundException(string code) : base(code)
    {
    }

    public NotFoundException(string objectName, string code) : base(code, $"{ErrorCode.NotFound} error occured on {objectName}.")
    {
    }
    
    public NotFoundException(string objectName, PartType parameterPartType, string code) : this(code, $"{ErrorCode.NotFound} error occured on {objectName} based on {parameterPartType}.")
    {
    }

    public NotFoundException(string objectName, string propertyName, string code) : this(code,  $"{ErrorCode.NotFound} error occured on {objectName}.{propertyName}.")
    {
    }
    
}
