using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Console.Query;
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

