using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Generator.Database;
using Parsis.Predicate.Sdk.Query;
using System.Runtime.InteropServices;

namespace Parsis.Predicate.Sdk.Builder.Database;
public abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryOperationType, DatabaseQueryPartCollection> where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
        set;
    }

    protected DatabaseQueryContext Context
    {
        get;
        set;
    }

    protected DatabaseQueryPartCollection QueryPartCollection
    {
        get;
        set;
    } = new();

    protected DatabaseQuery(IQueryContext context, DatabaseQueryOperationType queryType) : base(queryType)
    {
        Context = (DatabaseQueryContext)context;
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override async Task<DatabaseQueryPartCollection> Build(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        switch (QueryType)
        {
            case DatabaseQueryOperationType.Select:
                await GenerateColumnAsync(query);
                await GenerateWhereAsync(query);
                await GenerateOrderByAsync(query);
                await GenerateJoinAsync();
                await GeneratePagingAsync(query);
                await GenerateFunctionByClause();
                break;
            case DatabaseQueryOperationType.Insert:
                await GenerateInsertAsync(query);
                break;
            case DatabaseQueryOperationType.Update:
                await GenerateUpdateAsync(query);
                break;
            case DatabaseQueryOperationType.Delete:
                await GenerateDeleteAsync(query);
                break;
        }

        return QueryPartCollection;
    }

    protected abstract Task GenerateInsertAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateUpdateAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateDeleteAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateColumnAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateWhereAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GeneratePagingAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateOrderByAsync(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateJoinAsync();

    protected abstract Task GenerateFunctionByClause();
}

