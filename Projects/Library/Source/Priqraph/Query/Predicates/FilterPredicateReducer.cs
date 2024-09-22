using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query.Predicates;

internal class FilterPredicateReducer<TObject> : PredicateReducer<TObject> where TObject : IQueryableObject
{
    public override IQuery<TObject> Reduce(IQuery<TObject> query, ReduceType type) => base.Visit(query, type);

    protected override IQuery<TObject> Generate(IQuery<TObject> query) => query;

    protected override IQuery<TObject> Decrease(IQuery<TObject> query) => query;

    protected override IQuery<TObject> Merge(IQuery<TObject> query) => query;
}
