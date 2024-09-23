using Priqraph.DataType;
using System.Data.SqlClient;

namespace Priqraph.Generator.Database;
public  class DatabaseCommandQueryFragment : QueryFragment<CommandProperty>
{
    public Dictionary<string, object> CommandParts
    {
        get;
        set;
    } = new();

    public QueryOperationType OperationType
    {
        get;
        set;
    } = QueryOperationType.Add;

    public CommandValueType CommandValueType
    {
        get;
        set;
    } = CommandValueType.Record;

    public ICollection<SqlParameter> SqlParameters
    {
        get;
        set;
    } = new List<SqlParameter>();
}