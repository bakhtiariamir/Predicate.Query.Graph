using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query;

internal abstract class PredicateReducer<TObject, TQueryObject, TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible  
{
    protected virtual TQueryObject Visit(TQueryObject query, ReduceType type) =>
        type switch {
            ReduceType.Generate => Generate(query),
            ReduceType.Decrease => Decrease(query),
            ReduceType.Merge => Merge(query),
            _ => throw new NotImplementedException() //ToDo
        };

    public abstract TQueryObject Reduce(TQueryObject query, ReduceType type);

    protected abstract TQueryObject Generate(TQueryObject query);

    protected abstract TQueryObject Decrease(TQueryObject query);

    protected abstract TQueryObject Merge(TQueryObject query);
}
