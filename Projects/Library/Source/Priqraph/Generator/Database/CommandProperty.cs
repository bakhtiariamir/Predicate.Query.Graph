using System;

namespace Priqraph.Generator.Database
{
    public class CommandProperty
    {
        public ICollection<ColumnPropertyCollection>? ColumnPropertyCollections
        {
            get;
        }

        public CommandProperty(ICollection<ColumnPropertyCollection> columnPropertyCollections)
        {
            ColumnPropertyCollections = columnPropertyCollections;
        }
    }
}
