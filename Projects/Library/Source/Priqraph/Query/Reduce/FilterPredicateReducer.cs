using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query.Reduce;

internal class FilterPredicateReducer<TObject, TQueryObject, TEnum> : PredicateReducer<TObject, TQueryObject, TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible  
{
    public override TQueryObject Reduce(TQueryObject query, ReduceType type) => base.Visit(query, type);

    protected override TQueryObject Generate(TQueryObject query) => query;

    protected override TQueryObject Decrease(TQueryObject query) => query;

    protected override TQueryObject Merge(TQueryObject query) => query;
}
