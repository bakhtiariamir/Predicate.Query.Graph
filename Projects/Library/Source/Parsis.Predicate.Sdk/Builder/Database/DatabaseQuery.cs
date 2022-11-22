using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Database;
public abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryOperationType,  DatabaseQueryPartCollection<TObject>> where TObject : class
{
    protected QueryObjectJoining QueryObjectJoining
    {
        get;
    }

protected DatabaseQueryContext Context
    {
        get;
        set;
    }
    protected DatabaseQueryPartCollection<TObject> QueryPartCollection
    {
        get;
        set;
    } = new();

    protected DatabaseQuery(IQueryContext context, DatabaseQueryOperationType queryTypr) : base(queryTypr)
    {
        QueryObjectJoining = QueryObjectJoining.Init();
        Context = (DatabaseQueryContext)context;
    }

    public override async Task<DatabaseQueryPartCollection<TObject>> Build(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        switch (QueryType)
        {
            case DatabaseQueryOperationType.Select:
                await GenerateColumn(query);
                await GenerateWhereClause(query);
                break;
            case DatabaseQueryOperationType.Insert:
                Task.WaitAll(new[]
                {
                    GenerateColumn(query), GenerateWhereClause(query), GenerateOrderByClause(query), GenerateJoinClause(query)
                });
                break;
            case DatabaseQueryOperationType.Update:

            case DatabaseQueryOperationType.Delete:

                break;
        }

        return QueryPartCollection;
    }

    protected abstract Task GenerateColumn(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateWhereClause(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GeneratePagingClause(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateOrderByClause(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateJoinClause(QueryObject<TObject, DatabaseQueryOperationType> query);

    protected abstract Task GenerateGroupByClause(QueryObject<TObject, DatabaseQueryOperationType> query);
}

