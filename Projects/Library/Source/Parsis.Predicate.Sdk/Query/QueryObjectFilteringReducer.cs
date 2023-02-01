using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectFilteringReducer<TObject> : QueryObjectPartReducer<TObject> where TObject : IQueryableObject
{
    public override QueryObject<TObject> Reduce(QueryObject<TObject> query, ReduceType type)
    {
        return base.Visit(query, type);
    }

    protected override QueryObject<TObject> Generate(QueryObject<TObject> query)
    {
        return query;
    }

    protected override QueryObject<TObject> Decrease(QueryObject<TObject> query)
    {
        return query;
    }

    protected override QueryObject<TObject> Merge(QueryObject<TObject> query)
    {
        return query;
    }
}
