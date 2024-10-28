using Priqraph.Generator.Database;
using Priqraph.Setup;

namespace Priqraph.Generator.Neo4j;
public class Neo4jFilterQueryFragment : QueryFragment<FilterProperty>
{
    public QuerySetting? QuerySetting
    {
        get;
        protected set;
    }
}