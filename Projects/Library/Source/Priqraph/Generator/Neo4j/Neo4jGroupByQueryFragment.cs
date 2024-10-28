using Priqraph.Generator.Database;

namespace Priqraph.Generator.Neo4j;
public  class Neo4jGroupByQueryFragment : QueryFragment<GroupByProperty>
{
    public string? Having
    {
        get;
        protected set;
    }

}
