using Priqraph.Builder;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Neo4j.Builder;

namespace Priqraph.Neo4j.Manager;

public class Neo4jBuilder<TObject>(ICacheInfoCollection cacheInfoCollection) : INeo4jQueryBuilder<TObject>
    where TObject : IQueryableObject
{
    public IQueryObject<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryResult, Neo4jQueryOperationType> Build(bool isObjectQuery = true)
    {
        //return isObjectQuery  ? new Neo4jQuery<TObject>(cacheInfoCollection) : new Neo4jQueryableQueryObject<TObject>(cacheInfoCollection);
        return null;
    }
}
