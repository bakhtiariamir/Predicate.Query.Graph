using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Database;

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
                await GenerateJoinAsync();
                await GeneratePagingAsync(query);
                await GenerateFunctionByClause();
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

    protected abstract Task GenerateColumnAsync(QueryObject<TObject> query);

    protected abstract Task GenerateWhereAsync(QueryObject<TObject> query);

    protected abstract Task GeneratePagingAsync(QueryObject<TObject> query);

    protected abstract Task GenerateOrderByAsync(QueryObject<TObject> query);

    protected abstract Task GenerateJoinAsync();

    protected abstract Task GenerateFunctionByClause();
}
