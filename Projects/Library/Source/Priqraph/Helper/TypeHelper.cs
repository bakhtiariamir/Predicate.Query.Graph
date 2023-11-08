using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Helper;
public static class TypeHelper
{
    public static ColumnDataType ColumnDataType(this Type type) => type switch
    {
        not null when type == typeof(int) => DataType.ColumnDataType.Int,
        not null when type == typeof(long) => DataType.ColumnDataType.Long,
        not null when type == typeof(string) => DataType.ColumnDataType.String,
        not null when type == typeof(char) => DataType.ColumnDataType.Char,
        not null when type == typeof(decimal) => DataType.ColumnDataType.Decimal,
        not null when type == typeof(float) => DataType.ColumnDataType.Float,
        not null when type == typeof(double) => DataType.ColumnDataType.Double,
        not null when type == typeof(bool) => DataType.ColumnDataType.Boolean,
        not null when type == typeof(byte) => DataType.ColumnDataType.Byte,
        not null when type == typeof(uint) => DataType.ColumnDataType.UInt,
        not null when type == typeof(DateTime) => DataType.ColumnDataType.DateTime,
        not null when type == typeof(IQueryableObject) => DataType.ColumnDataType.Object,
        _ => throw new NotImplementedException(),
    };
}
