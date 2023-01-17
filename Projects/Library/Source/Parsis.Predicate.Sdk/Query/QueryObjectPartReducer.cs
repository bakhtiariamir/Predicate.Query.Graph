using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public abstract class QueryObjectPartReducer<TObject, TQueryType> where TObject : IQueryableObject
    where TQueryType : Enum
{
    protected virtual QueryObject<TObject, TQueryType> Visit(QueryObject<TObject, TQueryType> query, ReduceType type)
    {
        return type switch {
            ReduceType.Generate => Generate(query),
            ReduceType.Decrease => Decrease(query),
            ReduceType.Merge => Merge(query),
            _ => throw new NotImplementedException() //ToDo
        };
    }

    public abstract QueryObject<TObject, TQueryType> Reduce(QueryObject<TObject, TQueryType> query, ReduceType type);

    protected abstract QueryObject<TObject, TQueryType> Generate(QueryObject<TObject, TQueryType> query);

    protected abstract QueryObject<TObject, TQueryType> Decrease(QueryObject<TObject, TQueryType> query);

    protected abstract QueryObject<TObject, TQueryType> Merge(QueryObject<TObject, TQueryType> query);
}
