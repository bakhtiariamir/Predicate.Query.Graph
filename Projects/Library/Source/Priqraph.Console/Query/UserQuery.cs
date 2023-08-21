using Priqraph.Console.Model;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Console.Query;
public abstract class UserQuery<TOperationType, TResult> where TOperationType : Enum
{
    protected abstract IQueryOperation<User, TOperationType, DatabaseQueryPartCollection> Operation
    {
        get;
        init;
    }

    public IQuery<User, DatabaseQueryOperationType, TResult>? Query
    {
        get;
        private set;
    }

}

