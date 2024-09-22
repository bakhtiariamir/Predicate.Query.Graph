using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query;

internal abstract class PredicateReducer<TObject> where TObject : IQueryableObject
{
    protected virtual IQuery<TObject> Visit(IQuery<TObject> query, ReduceType type) =>
        type switch {
            ReduceType.Generate => Generate(query),
            ReduceType.Decrease => Decrease(query),
            ReduceType.Merge => Merge(query),
            _ => throw new NotImplementedException() //ToDo
        };

    public abstract IQuery<TObject> Reduce(IQuery<TObject> query, ReduceType type);

    protected abstract IQuery<TObject> Generate(IQuery<TObject> query);

    protected abstract IQuery<TObject> Decrease(IQuery<TObject> query);

    protected abstract IQuery<TObject> Merge(IQuery<TObject> query);
}
