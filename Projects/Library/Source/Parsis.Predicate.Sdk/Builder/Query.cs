using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;

public abstract class Query<TObject, TQueryType, TQueryResult> : IQuery<TObject, TQueryType, TQueryResult> where TObject : IQueryableObject
    where TQueryType : Enum
{
    public TQueryType QueryType
    {
        get;
        set;
    }

    protected Query(TQueryType queryType)
    {
        QueryType = queryType;
    }

    public abstract Task<TQueryResult> Build(QueryObject<TObject, TQueryType> query);
}
