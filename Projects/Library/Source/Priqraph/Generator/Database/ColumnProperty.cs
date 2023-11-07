using Priqraph.Contract;
using System;

namespace Priqraph.Generator.Database
{
    public class ColumnProperty
    {
        public IColumnPropertyInfo? ColumnPropertyInfo
        {
            get;
        }

        public object? Value
        {
            get;
            private set;
        }

        public ColumnProperty(IColumnPropertyInfo columnPropertyInfo)
        {
            ColumnPropertyInfo = columnPropertyInfo;
        }

        public ColumnProperty(object value)
        {
            Value = value;
        }

        public void SetValue(object? value) => Value = value;
    }
}
