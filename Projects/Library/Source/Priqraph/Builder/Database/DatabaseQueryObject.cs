using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Builder.Database;

public abstract class DatabaseQueryObject<TObject> : QueryObject<TObject, DatabaseQueryResult> where TObject : IQueryableObject
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

    protected DatabaseQueryObject(ICacheInfoCollection cacheInfoCollection)
    {
        QueryResult = new();
        Context = new DatabaseQueryContext(cacheInfoCollection);
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override DatabaseQueryResult Build(IQuery<TObject> query)
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

    protected abstract void GenerateInsert(IQuery<TObject> query);

    protected abstract void GenerateUpdate(IQuery<TObject> query);

    protected abstract void GenerateDelete(IQuery<TObject> query);

    protected abstract void GenerateColumn(IQuery<TObject> query, bool getCount = false);

    protected abstract void GenerateWhere(IQuery<TObject> query);

    protected abstract void GeneratePaging(IQuery<TObject> query);

    protected abstract void GenerateOrderBy(IQuery<TObject> query);

    protected abstract void GenerateJoin(IQuery<TObject> query);

    protected abstract void GenerateFunctionByClause();
}
