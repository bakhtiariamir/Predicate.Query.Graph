using Priqraph.Contract;
using System;

namespace Priqraph.Generator
{
    public class GroupByProperty
    {
        public IEnumerable<IColumnPropertyInfo> GroupingColumns
        {
            get;
        }

        public IEnumerable<FilterProperty>? GroupingHaving
        {
            get;
        }

        public GroupByProperty(IEnumerable<IColumnPropertyInfo> groupingColumns, IEnumerable<FilterProperty>? groupingHaving)
        {
            GroupingColumns = groupingColumns;
            GroupingHaving = groupingHaving;
        }
    }
}
