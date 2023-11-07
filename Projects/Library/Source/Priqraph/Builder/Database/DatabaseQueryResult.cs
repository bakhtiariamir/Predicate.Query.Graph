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

    public CommandQueryFragment? Command
    {
        get;
        set;
    }

    public ColumnQueryFragment? Columns
    {
        get;
        set;
    }

    public FilterQueryFragment? WhereClause
    {
        get;
        set;
    }

    public SortQueryFragment? OrderByClause
    {
        get;
        set;
    }

    public JoinQueryFragment? JoinClause
    {
        get;
        set;
    }

    public PageQueryFragment? Paging
    {
        get;
        set;
    }

    public GroupByQueryFragment? GroupByClause
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

public interface IQueryResult
{
}
