using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryPartCollection> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected DatabaseQueryContext Context
    {
        get;
    }

    protected DatabaseQueryPartCollection QueryPartCollection
    {
        get;
    }

    protected DatabaseQuery(IQueryContext context)
    {
        QueryPartCollection = new();
        Context = (DatabaseQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override async Task<DatabaseQueryPartCollection> Build(QueryObject<TObject> query)
    {
        switch (query.QueryOperationType)
        {
            case QueryOperationType.GetData:
                await GenerateColumnAsync(query);
                await GenerateWhereAsync(query);
                await GenerateOrderByAsync(query);
                await GenerateJoinAsync(query);
                await GeneratePagingAsync(query);
                await GenerateFunctionByClause();
                break;
            case QueryOperationType.GetCount:
                await GenerateColumnAsync(query, true);
                await GenerateWhereAsync(query);
                await GenerateJoinAsync(query);
                break;
            case QueryOperationType.Add:
                await GenerateInsertAsync(query);
                break;
            case QueryOperationType.Edit:
                await GenerateUpdateAsync(query);
                break;
            case QueryOperationType.Remove:
                await GenerateDeleteAsync(query);
                break;
        }

        return QueryPartCollection;
    }

    protected abstract Task GenerateInsertAsync(QueryObject<TObject> query);

    protected abstract Task GenerateUpdateAsync(QueryObject<TObject> query);

    protected abstract Task GenerateDeleteAsync(QueryObject<TObject> query);

    protected abstract Task GenerateColumnAsync(QueryObject<TObject> query, bool getCount = false);

    protected abstract Task GenerateWhereAsync(QueryObject<TObject> query);

    protected abstract Task GeneratePagingAsync(QueryObject<TObject> query);

    protected abstract Task GenerateOrderByAsync(QueryObject<TObject> query);

    protected abstract Task GenerateJoinAsync(QueryObject<TObject> query);

    protected abstract Task GenerateFunctionByClause();
}
