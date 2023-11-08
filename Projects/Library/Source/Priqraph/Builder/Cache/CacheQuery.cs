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

    public override CacheQueryResult Build(IQueryObject<TObject> query)
    {
        switch (query.QueryOperationType)
        {
            case QueryOperationType.GetData:
                GenerateWhere(query);
                GenerateOrderBy(query);
                //await GenerateJoinAsync();
                GeneratePaging(query);
                break;
            case QueryOperationType.GetCount:
                GenerateWhere(query);
                //await GenerateJoinAsync();
                break;
            case QueryOperationType.Add:
                GenerateAdd(query);
                break;
            case QueryOperationType.Edit:
                GenerateUpdate(query);
                break;
            case QueryOperationType.Remove:
                GenerateRemove(query);
                break;
        }

        return QueryResult;
    }

    protected abstract Task GenerateAdd(IQueryObject<TObject> query);

    protected abstract Task GenerateUpdate(IQueryObject<TObject> query);

    protected abstract Task GenerateRemove(IQueryObject<TObject> query);

    protected abstract Task GenerateWhere(IQueryObject<TObject> query);

    protected abstract Task GeneratePaging(IQueryObject<TObject> query);

    protected abstract Task GenerateOrderBy(IQueryObject<TObject> query);

    //protected abstract Task GenerateJoinAsync();

}
