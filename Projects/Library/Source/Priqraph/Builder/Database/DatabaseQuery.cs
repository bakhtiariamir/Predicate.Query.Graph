using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

internal abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryResult> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected DatabaseQueryContext Context
    {
        get;
    }

    protected DatabaseQueryResult QueryResult
    {
        get;
    }

    protected DatabaseQuery(IQueryContext context)
    {
        QueryResult = new();
        Context = (DatabaseQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override async Task<DatabaseQueryResult> Build(IQueryObject<TObject> query)
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

        return QueryResult;
    }

    protected abstract Task GenerateInsertAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateUpdateAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateDeleteAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateColumnAsync(IQueryObject<TObject> query, bool getCount = false);

    protected abstract Task GenerateWhereAsync(IQueryObject<TObject> query);

    protected abstract Task GeneratePagingAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateOrderByAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateJoinAsync(IQueryObject<TObject> query);

    protected abstract Task GenerateFunctionByClause();
}
