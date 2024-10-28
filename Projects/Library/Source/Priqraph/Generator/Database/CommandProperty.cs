using System;

namespace Priqraph.Generator.Database
{
    public class CommandProperty(ICollection<ColumnPropertyCollection> columnPropertyCollections)
    {
        public ICollection<ColumnPropertyCollection>? ColumnPropertyCollections
        {
            get;
        } = columnPropertyCollections;
    }
}
