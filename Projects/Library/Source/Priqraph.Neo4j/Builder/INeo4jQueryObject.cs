using Priqraph.Builder;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Neo4j.Builder;

public interface INeo4jQueryObject<TObject> : IQueryObject<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryResult, Neo4jQueryOperationType> 
    where TObject : IQueryableObject
{

}

public interface INeo4jQueryableQueryObject<TObject> : IQueryObject<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryResult, Neo4jQueryOperationType> 
    where TObject : IQueryableObject
{

}

public interface INeo4jQueryBuilder<TObject> : IQueryBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryResult, Neo4jQueryOperationType> 
    where TObject : IQueryableObject
{
}