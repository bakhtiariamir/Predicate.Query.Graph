using Priqraph.Console.Model;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Console.Query;
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

