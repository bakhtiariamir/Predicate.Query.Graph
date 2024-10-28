using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

public abstract class SqlQueryObject<TObject, TObjectQuery> : QueryObject<TObject, TObjectQuery, DatabaseQueryResult, DatabaseQueryOperationType> 
    where TObject : IQueryableObject
    where TObjectQuery : ISqlQuery<TObject, DatabaseQueryOperationType>
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

    protected SqlQueryObject(ICacheInfoCollection cacheInfoCollection)
    {
        QueryResult = new();
        Context = new DatabaseQueryContext(cacheInfoCollection);
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override DatabaseQueryResult Build(TObjectQuery query)
    {
        switch (query.DatabaseQueryOperationType)
        {
            case DatabaseQueryOperationType.GetData:
                GenerateColumn(query);
                GenerateWhere(query);
                GenerateOrderBy(query);
                GenerateJoin(query);
                GeneratePaging(query);
                GenerateFunctionByClause();
                break;
            case DatabaseQueryOperationType.GetCount:
                GenerateColumn(query, true);
                GenerateWhere(query);
                GenerateJoin(query);
                break;
            case DatabaseQueryOperationType.Add:
                GenerateInsert(query);
                break;
            case DatabaseQueryOperationType.Edit:
                GenerateUpdate(query);
                break;
            case DatabaseQueryOperationType.Remove:
                GenerateDelete(query);
                break;
        }

        return QueryResult;
    }

    protected abstract void GenerateInsert(TObjectQuery query);

    protected abstract void GenerateUpdate(TObjectQuery query);

    protected abstract void GenerateDelete(TObjectQuery query);

    protected abstract void GenerateColumn(TObjectQuery query, bool getCount = false);

    protected abstract void GenerateWhere(TObjectQuery query);

    protected abstract void GeneratePaging(TObjectQuery query);

    protected abstract void GenerateOrderBy(TObjectQuery query);

    protected abstract void GenerateJoin(TObjectQuery query);

    protected abstract void GenerateFunctionByClause();
}
