using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Neo4j.Builder;

public interface INeo4jQuery<TObject, in TEnum> : IQuery<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{
    Neo4jQueryOperationType QueryOperationType
    {
        get;
        set;
    }
}