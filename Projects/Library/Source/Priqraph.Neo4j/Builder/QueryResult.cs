using Priqraph.Contract;
using Priqraph.Neo4j.Generator;

namespace Priqraph.Builder;
    public class Neo4jQueryResult : IQueryResult
    {
    public IDatabaseObjectInfo? DatabaseObjectInfo
    {
        get;
        set;
    }

    public Neo4JCommandQueryFragment? CommandFragment
    {
        get;
        set;
    }

    public Neo4jColumnQueryFragment? ColumnFragment
    {
        get;
        set;
    }

    public Neo4jFilterQueryFragment? FilterFragment
    {
        get;
        set;
    }

    public Neo4jSortQueryFragment? SortFragment
    {
        get;
        set;
    }

    public Neo4JJoinQueryFragment? JoinFragment
    {
        get;
        set;
    }

    public Neo4jPageQueryFragment? PageFragment
    {
        get;
        set;
    }

    public Neo4jGroupByQueryFragment? GroupByFragment
    {
        get;
        set;
    }

    public Neo4jQueryResult? ResultQuery
    {
        get;
        set;
    }
}