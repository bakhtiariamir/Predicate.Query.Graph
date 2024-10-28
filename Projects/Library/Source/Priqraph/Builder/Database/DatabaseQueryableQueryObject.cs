using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQueryableQueryObject<TObject, TObjectQuery, TQueryResult, TEnum>(
    ICacheInfoCollection cacheInfoCollection) : QueryObject<TObject, TObjectQuery, TQueryResult, TEnum>
    where TObject : IQueryableObject
    where TObjectQuery : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
    where TQueryResult : IQueryResult
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    } = new();

    protected DatabaseQueryContext Context
    {
        get;
    } = new(cacheInfoCollection);

    protected TQueryResult QueryResult
    {
        get;
        set;
    }
}
