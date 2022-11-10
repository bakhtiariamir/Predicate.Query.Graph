using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Console.Query;
public abstract class PersonQuery<TResult>
{
    protected abstract IQueryBuilder<Person, TResult>? QueryBuilder
    {
        get;
    }

    protected abstract QueryObject<Person>? QueryObject
    {
        get;
    }

    public IQuery<Person, TResult>? Query
    {
        get;
        private set;
    }

    public virtual async Task SelectQueryAsync() => Query = await QueryBuilder.Build(QueryObject);

}

