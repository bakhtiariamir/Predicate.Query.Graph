using Priqraph.Contract;
using Priqraph.DataType;
using System.Data;
using System.Data.SqlClient;

namespace Priqraph.Helper;


public static class BaseQueryParameterHelper
{
    public static IEnumerable<BaseQueryParameter> ArrayParameters<TDataType>(string baseParameterName, IEnumerable<TDataType> values, ColumnDataType columnDataType, int? index = 0)
    {
        var dbType = columnDataType.GetSqlDbType();

        foreach (var value in values)
        {
            var parameterName = $"@{baseParameterName}_{index++}";
            var parameter = new BaseQueryParameter(parameterName, dbType)
            {
                Value = value
            };

            yield return parameter;
        }
    }
}
public static class SqlParameterHelper
{
    public static SqlDbType GetSqlDbType(this ColumnDataType columnDataType) => columnDataType switch {
        ColumnDataType.Char => SqlDbType.Char,
        ColumnDataType.String => SqlDbType.NVarChar,
        ColumnDataType.Boolean => SqlDbType.Bit,
        ColumnDataType.Byte => SqlDbType.TinyInt,
        ColumnDataType.Int => SqlDbType.Int,
        ColumnDataType.UInt => SqlDbType.Int,
        ColumnDataType.Long => SqlDbType.BigInt,
        ColumnDataType.Float => SqlDbType.Float,
        ColumnDataType.Decimal => SqlDbType.Decimal,
        ColumnDataType.Double => SqlDbType.Decimal,
        ColumnDataType.DateTime => SqlDbType.DateTime,
        ColumnDataType.Object => SqlDbType.VarBinary,
        ColumnDataType.Binary => SqlDbType.VarBinary,
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };

    public static ColumnDataType GetColumnDataType(this Type type) => type switch {
        not null when type == typeof(int) => ColumnDataType.Int,
        not null when type == typeof(long) => ColumnDataType.Long,
        not null when type == typeof(string) => ColumnDataType.String,
        not null when type == typeof(char) => ColumnDataType.Char,
        not null when type == typeof(decimal) => ColumnDataType.Decimal,
        not null when type == typeof(float) => ColumnDataType.Float,
        not null when type == typeof(double) => ColumnDataType.Double,
        not null when type == typeof(bool) => ColumnDataType.Boolean,
        not null when type == typeof(byte) => ColumnDataType.Byte,
        not null when type == typeof(uint) => ColumnDataType.UInt,
        not null when type == typeof(DateTime) => ColumnDataType.DateTime,
        not null when type == typeof(IQueryableObject) => ColumnDataType.Object,
        _ => throw new NotImplementedException(),
    };

    public static string GetTextBasedOnSqlDbType(this ColumnDataType columnDataType, object? value) => columnDataType switch {
        ColumnDataType.Char => value != null ? $"N'{value}'" : "NULL",
        ColumnDataType.String => value != null ? $"N'{value}'" : "NULL",
        ColumnDataType.Boolean => value != null ? $"{value}" : "NULL",
        ColumnDataType.Byte => value != null ? $"'{value}'" : "NULL",
        ColumnDataType.Int => value != null ? $"{value}" : "NULL",
        ColumnDataType.UInt => value != null ? $"{value}" : "NULL",
        ColumnDataType.Long => value != null ? $"{value}" : "NULL",
        ColumnDataType.Float => value != null ? $"{value}" : "NULL",
        ColumnDataType.Decimal => value != null ? $"{value}" : "NULL",
        ColumnDataType.Double => value != null ? $"{value}" : "NULL",
        ColumnDataType.DateTime => value != null ? $"'{value}'" : "NULL",
        ColumnDataType.Object => value != null ? $"'{value}'" : "NULL",
        ColumnDataType.Binary => value != null ? $"'{value}'" : "NULL",
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };

    public static string GetParameterStringBasedOnSqlDbType(this ColumnDataType columnDataType, string parameterName, object? value) => columnDataType switch {
        ColumnDataType.Char => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.String => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Boolean => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Byte => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Int => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.UInt => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Long => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Float => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Decimal => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Double => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.DateTime => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Object => value != null ? $"{parameterName}" : $"{parameterName}",
        ColumnDataType.Structure => value != null ? $"({parameterName})" : $"({parameterName})",
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };

    public static string GetParameterPhrase(this IColumnPropertyInfo columnPropertyInfo, string parameterName) => $"[{columnPropertyInfo.ColumnName}] = {parameterName}";

    public static IEnumerable<SqlParameter> ArrayParameters<TDataType>(string baseParameterName, IEnumerable<TDataType> values, ColumnDataType columnDataType, int? index = 0)
    {
        var dbType = columnDataType.GetSqlDbType();

        foreach (var value in values)
        {
            var parameterName = $"@{baseParameterName}_{index++}";
            var parameter = new SqlParameter(parameterName, dbType) {
                Value = value
            };

            yield return parameter;
        }
    }

    public static string ArrayParameterNames(this IColumnPropertyInfo column, string baseParameterName, int valueCount, int? index = 0)
    {
        var parameterNames = new List<string>();
        for (var i = index; i < valueCount; i++)
        {
            var parameterName = $"@{baseParameterName}_{i}";
            parameterNames.Add(parameterName);
        }

        return $"{column.ColumnName} IN ({string.Join(", ", parameterNames)})";
    }
}
