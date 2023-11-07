using Priqraph.Contract;
using System;

namespace Priqraph.Generator.Database
{
    public class GroupByProperty
    {
        public IEnumerable<IColumnPropertyInfo> GroupingColumns
        {
            get;
        }

        public IEnumerable<FilterClause>? GroupingHaving
        {
            get;
        }

        public GroupByProperty(IEnumerable<IColumnPropertyInfo> groupingColumns, IEnumerable<FilterClause>? groupingHaving)
        {
            GroupingColumns = groupingColumns;
            GroupingHaving = groupingHaving;
        }
    }
}
