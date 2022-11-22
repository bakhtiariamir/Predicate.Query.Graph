using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectSelectingReducer<TObject, TQueryType> : QueryObjectPartReducer<TObject, TQueryType> where TObject : class where TQueryType : Enum
{
    public override QueryObject<TObject, TQueryType> Reduce(QueryObject<TObject, TQueryType> query, ReduceType type)
    {
        return base.Visit(query, type);
    }

    protected override QueryObject<TObject, TQueryType> Generate(QueryObject<TObject, TQueryType> query)
    {
        return query;
    }

    protected override QueryObject<TObject, TQueryType> Decrease(QueryObject<TObject, TQueryType> query)
    {
        return query;
    }

    protected override QueryObject<TObject, TQueryType> Merge(QueryObject<TObject, TQueryType> query)
    {
        return query;
    }
}
