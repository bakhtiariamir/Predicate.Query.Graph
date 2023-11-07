using System;

namespace Priqraph.Generator.Database
{
    public class ColumnPropertyCollection
    {
        public ICollection<ColumnProperty>? ColumnProperties
        {
            get;
        }

        public IEnumerable<object>? Records
        {
            get;
            private set;
        }

        public ColumnPropertyCollection(ICollection<ColumnProperty> columnProperties)
        {
            ColumnProperties = columnProperties;
        }

        public ColumnPropertyCollection(IEnumerable<object>? records)
        {
            Records = records;
        }

        public void SetData(IEnumerable<object>? records) => Records = records;

        public void AddColumn(ColumnProperty columnProperty) => ColumnProperties?.Add(columnProperty);
    }
}
