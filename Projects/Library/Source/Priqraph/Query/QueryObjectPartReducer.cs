using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query;

public abstract class QueryObjectPartReducer<TObject> where TObject : IQueryableObject
{
    protected virtual QueryObject<TObject> Visit(QueryObject<TObject> query, ReduceType type)
    {
        return type switch {
            ReduceType.Generate => Generate(query),
            ReduceType.Decrease => Decrease(query),
            ReduceType.Merge => Merge(query),
            _ => throw new NotImplementedException() //ToDo
        };
    }

    public abstract QueryObject<TObject> Reduce(QueryObject<TObject> query, ReduceType type);

    protected abstract QueryObject<TObject> Generate(QueryObject<TObject> query);

    protected abstract QueryObject<TObject> Decrease(QueryObject<TObject> query);

    protected abstract QueryObject<TObject> Merge(QueryObject<TObject> query);
}
