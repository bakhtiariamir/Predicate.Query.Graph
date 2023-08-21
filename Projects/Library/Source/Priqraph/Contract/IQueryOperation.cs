using Priqraph.DataType;
using Priqraph.Query;

namespace Priqraph.Contract;

public interface IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    Task<TResult> RunAsync(QueryObject<TObject> queryObject);

    void Init(QueryOperationType queryOperationType);

    QueryObjectBuilder<TObject>? QueryBuilder
    {
        get;
    }
}
