using Priqraph.DataType;
using Priqraph.Generator.Database;

namespace Priqraph.Generator.Neo4j;

public  class Neo4jCommandQueryFragment : QueryFragment<CommandProperty>
{
    public Dictionary<string, object> CommandParts
    {
        get;
        set;
    } = new();

    public Neo4jQueryOperationType OperationType
    {
        get;
        set;
    } = Neo4jQueryOperationType.InsertNode;

    public CommandValueType CommandValueType
    {
        get;
        set;
    } = CommandValueType.Record;

    public ICollection<Neo4jParameter> Parameters
    {
        get;
        set;
    } = new List<Neo4jParameter>();
}