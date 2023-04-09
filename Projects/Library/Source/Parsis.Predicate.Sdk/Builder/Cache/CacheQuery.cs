using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Cache;

public abstract class CacheQuery<TObject> : Query<TObject, CacheQueryPartCollection> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected CacheQueryContext Context
    {
        get;
    }

    protected CacheQueryPartCollection QueryPartCollection
    {
        get;
    }

    protected CacheQuery(IQueryContext context)
    {
        QueryPartCollection = new();
        Context = (CacheQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override async Task<CacheQueryPartCollection> Build(QueryObject<TObject> query)
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

        return QueryPartCollection;
    }

    protected abstract Task GenerateAddAsync(QueryObject<TObject> query);

    protected abstract Task GenerateUpdateAsync(QueryObject<TObject> query);

    protected abstract Task GenerateRemoveAsync(QueryObject<TObject> query);

    protected abstract Task GenerateWhereAsync(QueryObject<TObject> query);

    protected abstract Task GeneratePagingAsync(QueryObject<TObject> query);

    protected abstract Task GenerateOrderByAsync(QueryObject<TObject> query);

    //protected abstract Task GenerateJoinAsync();

}
