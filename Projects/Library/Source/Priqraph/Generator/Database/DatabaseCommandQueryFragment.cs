using Priqraph.DataType;

namespace Priqraph.Generator.Database;
public  class DatabaseCommandQueryFragment<TParameter> : QueryFragment<CommandProperty>
{
    public Dictionary<string, object> CommandParts
    {
        get;
        set;
    } = new();

    public DatabaseQueryOperationType OperationType
    {
        get;
        set;
    } = DatabaseQueryOperationType.Add;

    public CommandValueType CommandValueType
    {
        get;
        set;
    } = CommandValueType.Record;

    public ICollection<TParameter> Parameters
    {
        get;
        set;
    } = new List<TParameter();
}