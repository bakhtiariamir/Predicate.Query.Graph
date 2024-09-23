using Priqraph.Setup;

namespace Priqraph.Generator.Database
{
    public class DatabaseFilterQueryFragment : QueryFragment<FilterProperty>
    {
        public QuerySetting? QuerySetting
        {
            get;
            protected set;
        }
    }
}