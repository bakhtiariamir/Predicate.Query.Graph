using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class Query<TObject, TQueryResult> : IQuery<TObject, TQueryResult> where TObject : class
{
    public QueryObject<TObject> ObjectQuery
    {
        get;
    }

    protected Query(QueryObject<TObject> objectQuery)
    {
        ObjectQuery = objectQuery;
    }

    public abstract Task<TQueryResult> Build();
}

