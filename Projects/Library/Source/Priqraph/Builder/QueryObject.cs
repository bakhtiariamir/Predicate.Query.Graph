using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class QueryObject<TObject, TObjectQuery, TQueryResult, TEnum> : IQueryObject<TObject, TObjectQuery, TQueryResult, TEnum> 
    where TObject : IQueryableObject
    where TObjectQuery : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
{
    public abstract TObjectQuery Query
    {
        get;
    }
    public abstract TQueryResult Build(TObjectQuery query);
}
