using Priqraph.Contract;
using Priqraph.Generator.Database;

namespace Priqraph.Builder.Database;

public class DatabaseQueryResult : IQueryResult
{
    public IDatabaseObjectInfo? DatabaseObjectInfo
    {
        get;
        set;
    }

    public DatabaseCommandQueryFragment? CommandFragment
    {
        get;
        set;
    }

    public DatabaseColumnQueryFragment? ColumnFragment
    {
        get;
        set;
    }

    public DatabaseFilterQueryFragment? FilterFragment
    {
        get;
        set;
    }

    public DatabaseSortQueryFragment? SortFragment
    {
        get;
        set;
    }

    public DatabaseJoinQueryFragment? JoinFragment
    {
        get;
        set;
    }

    public DatabasePageQueryFragment? PageFragment
    {
        get;
        set;
    }

    public DatabaseGroupByQueryFragment? GroupByFragment
    {
        get;
        set;
    }

    public DatabaseQueryResult? ResultQuery
    {
        get;
        set;
    }
}