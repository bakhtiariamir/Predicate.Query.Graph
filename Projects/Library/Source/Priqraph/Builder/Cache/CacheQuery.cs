using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Cache;

internal abstract class CacheQuery<TObject> : Query<TObject, CacheQueryResult> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected CacheQueryContext Context
    {
        get;
    }

    protected CacheQueryResult QueryResult
    {
        get;
    }

    protected CacheQuery(IQueryContext context)
    {
        QueryResult = new();
        Context = (CacheQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override async Task<CacheQueryResult> Build(IQueryObject<TObject> query)
    {
        switch (query.QueryOperationType)
        {
            case QueryOperationType.GetData:
                await GenerateWhereAsync(query);
                await GenerateOrderByAsync(query);
                //await GenerateJoinAsync();
                await GeneratePagingAsync(query);
                break;
            case QueryOperationType.GetCount:
                await GenerateWhereAsync(query);
                //await GenerateJoinAsync();
                break;
            case QueryOperationType.Add:
                await GenerateAddAsync(query);
                break;
            case QueryOperationType.Edit:
                await GenerateUpdateAsync(query);
                break;
            case QueryOperationType.Remove:
                await GenerateRemoveAsync(query);
                break;
        }

        return QueryResult;
    }

    protected abstract Task GenerateAddAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateUpdateAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateRemoveAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateWhereAsync(IQueryObject<TObject> query);

    protected abstract Task GeneratePagingAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateOrderByAsync(IQueryObject<TObject> query);

    //protected abstract Task GenerateJoinAsync();

}
