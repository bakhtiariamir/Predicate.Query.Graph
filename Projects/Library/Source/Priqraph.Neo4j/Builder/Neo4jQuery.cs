using Priqraph.Builder;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Neo4j.Builder;

public abstract class Neo4jQuery<TObject> : QueryObject<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryResult, Neo4jQueryOperationType> 
    where TObject : IQueryableObject
{
    protected List<IColumnPropertyInfo> JoinColumns
    {
        get;
    }

    protected Neo4jQueryContext Context
    {
        get;
    }

    protected Neo4jQueryResult QueryResult
    {
        get;
    }

    protected Neo4jQuery(ICacheInfoCollection cacheInfoCollection)
    {
        QueryResult = new();
        Context = new Neo4jQueryContext(cacheInfoCollection);
        JoinColumns = new List<IColumnPropertyInfo>();
    }

    public override Neo4jQueryResult Build(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        switch (query.QueryOperationType)
        {
            case Neo4jQueryOperationType.InsertNode:
                break;
        }

        return QueryResult;
    }

    protected abstract void GenerateInsert(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GenerateUpdate(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GenerateDelete(INeo4jQuery<TObject,Neo4jQueryOperationType> query);

    protected abstract void GenerateColumn(INeo4jQuery<TObject, Neo4jQueryOperationType> query, bool getCount = false);

    protected abstract void GenerateWhere(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GeneratePaging(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GenerateOrderBy(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GenerateJoin(INeo4jQuery<TObject, Neo4jQueryOperationType> query);

    protected abstract void GenerateFunctionByClause();
}
