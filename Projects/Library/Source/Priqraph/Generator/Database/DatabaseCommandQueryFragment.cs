using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Builders;
using Priqraph.Setup;
using System.Data.SqlClient;

namespace Priqraph.Generator.Database;
public class DatabaseCommandQueryFragment : QueryFragment<CommandProperty>
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


public class DatabaseColumnQueryFragment : QueryFragment<ICollection<IColumnPropertyInfo>>
{

}


public class DatabaseFilterQueryFragment : QueryFragment<FilterProperty>
{
    public QuerySetting? QuerySetting
    {
        get;
        protected set;
    }
}

public class DatabaseGroupByQueryFragment : QueryFragment<GroupByProperty>
{
    public string? Having
    {
        get;
        protected set;
    }

}

public class DatabaseJoinQueryFragment : QueryFragment<ICollection<JoinProperty>>
{

}

public class DatabasePageQueryFragment : QueryFragment<PageClause>
{

}

public class DatabaseSortQueryFragment : QueryFragment<ICollection<SortProperty>>
{

}