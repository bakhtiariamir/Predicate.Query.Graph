using System.Data.SqlClient;
using Priqraph.Builder;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Neo4j.Extensions;

namespace Priqraph.Neo4j.Manager;

public class Neo4jQueryGenerator : IObjectQueryGenerator<Neo4jParameter, Neo4jObjectQuery, Neo4jQueryResult>
{

    public Neo4jObjectQuery? GenerateResult(Neo4jQueryOperationType operationType, Neo4jQueryResult query)
    {
        var phrase = string.Empty;
        ICollection<Neo4jParameter>? parameters = null;
        switch (operationType)
        {
            case Neo4jQueryOperationType.FindNode:
                phrase = query.FindNode(out parameters);
                break;
            case Neo4jQueryOperationType.InsertNode:
                phrase = query.Command(out parameters);
                break;
        }

        return new Neo4jObjectQuery(parameters, phrase);
    }

    public Neo4jObjectQuery? GenerateResult(DatabaseQueryOperationType operationType, Neo4jQueryResult query)
    {
        throw new NotImplementedException();
    }
}