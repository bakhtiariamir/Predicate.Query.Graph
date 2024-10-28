using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Generator
{
    public class SortProperty
    {
        public IColumnPropertyInfo ColumnPropertyInfo
        {
            get;
        }

        public DirectionType DirectionType
        {
            get;
        }

        public SortProperty(IColumnPropertyInfo columnPropertyInfo, DirectionType directionType = DirectionType.Asc)
        {
            ColumnPropertyInfo = columnPropertyInfo;
            DirectionType = directionType;
        }
    }
}
