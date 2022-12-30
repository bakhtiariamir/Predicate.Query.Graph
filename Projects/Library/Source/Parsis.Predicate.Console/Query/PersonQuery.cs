using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Console.Query;
public abstract class PersonQuery<TOperationType, TResult> where TOperationType : Enum
{
    protected abstract IQueryOperation<Person, TOperationType, DatabaseQueryPartCollection> Operation
    {
        get;
        init;
    }

    public IQuery<Person, DatabaseQueryOperationType, TResult>? Query
    {
        get;
        private set;
    }

}

