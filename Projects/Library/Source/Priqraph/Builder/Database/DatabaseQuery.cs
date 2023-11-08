using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryResult> where TObject : IQueryableObject
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

    protected DatabaseQuery(ICacheInfoCollection cacheInfoCollection)
    {
        QueryResult = new();
        Context = new DatabaseQueryContext(cacheInfoCollection);
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override DatabaseQueryResult Build(IQueryObject<TObject> query)
    {
        switch (query.QueryOperationType)
        {
            case QueryOperationType.GetData:
                GenerateColumn(query);
                GenerateWhere(query);
                GenerateOrderBy(query);
                GenerateJoin(query);
                GeneratePaging(query);
                GenerateFunctionByClause();
                break;
            case QueryOperationType.GetCount:
                GenerateColumn(query, true);
                GenerateWhere(query);
                GenerateJoin(query);
                break;
            case QueryOperationType.Add:
                GenerateInsert(query);
                break;
            case QueryOperationType.Edit:
                GenerateUpdate(query);
                break;
            case QueryOperationType.Remove:
                GenerateDelete(query);
                break;
        }

        return QueryResult;
    }

    protected abstract void GenerateInsert(IQueryObject<TObject> query);

    protected abstract void GenerateUpdate(IQueryObject<TObject> query);

    protected abstract void GenerateDelete(IQueryObject<TObject> query);

    protected abstract void GenerateColumn(IQueryObject<TObject> query, bool getCount = false);

    protected abstract void GenerateWhere(IQueryObject<TObject> query);

    protected abstract void GeneratePaging(IQueryObject<TObject> query);

    protected abstract void GenerateOrderBy(IQueryObject<TObject> query);

    protected abstract void GenerateJoin(IQueryObject<TObject> query);

    protected abstract void GenerateFunctionByClause();
}
