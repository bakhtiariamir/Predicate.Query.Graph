using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query.Predicates;

public class FilterPredicateReducer<TObject> : PredicateReducer<TObject> where TObject : IQueryableObject
{
    public override IQueryObject<TObject> Reduce(IQueryObject<TObject> query, ReduceType type)
    {
        return base.Visit(query, type);
    }

    protected override IQueryObject<TObject> Generate(IQueryObject<TObject> query)
    {
        return query;
    }

    protected override IQueryObject<TObject> Decrease(IQueryObject<TObject> query)
    {
        return query;
    }

    protected override IQueryObject<TObject> Merge(IQueryObject<TObject> query)
    {
        return query;
    }
}
