using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query;

public abstract class PredicateReducer<TObject> where TObject : IQueryableObject
{
    protected virtual IQueryObject<TObject> Visit(IQueryObject<TObject> query, ReduceType type) =>
        type switch {
            ReduceType.Generate => Generate(query),
            ReduceType.Decrease => Decrease(query),
            ReduceType.Merge => Merge(query),
            _ => throw new NotImplementedException() //ToDo
        };

    public abstract IQueryObject<TObject> Reduce(IQueryObject<TObject> query, ReduceType type);

    protected abstract IQueryObject<TObject> Generate(IQueryObject<TObject> query);

    protected abstract IQueryObject<TObject> Decrease(IQueryObject<TObject> query);

    protected abstract IQueryObject<TObject> Merge(IQueryObject<TObject> query);
}
