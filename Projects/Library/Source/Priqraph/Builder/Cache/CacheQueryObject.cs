using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Cache;

internal abstract class CacheQueryObject<TObject> : QueryObject<TObject, CacheQueryResult> where TObject : IQueryableObject
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

    protected CacheQueryObject(IQueryContext context)
    {
        QueryResult = new();
        Context = (CacheQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override CacheQueryResult Build(IQuery<TObject> query)
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

    protected abstract Task GenerateAdd(IQuery<TObject> query);

    protected abstract Task GenerateUpdate(IQuery<TObject> query);

    protected abstract Task GenerateRemove(IQuery<TObject> query);

    protected abstract Task GenerateWhere(IQuery<TObject> query);

    protected abstract Task GeneratePaging(IQuery<TObject> query);

    protected abstract Task GenerateOrderBy(IQuery<TObject> query);

    //protected abstract Task GenerateJoinAsync();

}
