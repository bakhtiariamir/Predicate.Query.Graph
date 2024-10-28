using Priqraph.Builder.Database;

namespace Priqraph.Contract;

public interface IQueryObject<TObject, TObjectQuery, out TQueryResult, TEnum> 
    where TObject : IQueryableObject 
    where TObjectQuery : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
{
    TObjectQuery Query
    {
        get;
    }
    TQueryResult Build(TObjectQuery query);
}
