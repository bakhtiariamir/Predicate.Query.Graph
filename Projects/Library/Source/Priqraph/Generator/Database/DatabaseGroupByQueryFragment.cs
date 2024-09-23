namespace Priqraph.Generator.Database
{
    public  class DatabaseGroupByQueryFragment : QueryFragment<GroupByProperty>
    {
        public string? Having
        {
            get;
            protected set;
        }

    }
}