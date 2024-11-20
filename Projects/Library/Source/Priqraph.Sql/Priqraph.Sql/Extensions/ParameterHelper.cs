using Priqraph.Contract;
using Priqraph.DataType;
using System.Data;
using System.Data.SqlClient;

namespace Priqraph.Sql.Extensions;
public static class ParameterHelper
{
    public static SqlDbType SqlDbType(this ColumnDataType columnDataType) => columnDataType switch
    {
        DataType.ColumnDataType.Char => System.Data.SqlDbType.Char,
        DataType.ColumnDataType.String => System.Data.SqlDbType.NVarChar,
        DataType.ColumnDataType.Boolean => System.Data.SqlDbType.Bit,
        DataType.ColumnDataType.Byte => System.Data.SqlDbType.TinyInt,
        DataType.ColumnDataType.Int => System.Data.SqlDbType.Int,
        DataType.ColumnDataType.UInt => System.Data.SqlDbType.Int,
        DataType.ColumnDataType.Long => System.Data.SqlDbType.BigInt,
        DataType.ColumnDataType.Float => System.Data.SqlDbType.Float,
        DataType.ColumnDataType.Decimal => System.Data.SqlDbType.Decimal,
        DataType.ColumnDataType.Double => System.Data.SqlDbType.Decimal,
        DataType.ColumnDataType.DateTime => System.Data.SqlDbType.DateTime,
        DataType.ColumnDataType.Object => System.Data.SqlDbType.VarBinary,
        DataType.ColumnDataType.Binary => System.Data.SqlDbType.VarBinary,
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };
    
    

    public static string GetTextBasedOnSqlDbType(this ColumnDataType columnDataType, object? value) => columnDataType switch
    {
        DataType.ColumnDataType.Char => value != null ? $"N'{value}'" : "NULL",
        DataType.ColumnDataType.String => value != null ? $"N'{value}'" : "NULL",
        DataType.ColumnDataType.Boolean => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.Byte => value != null ? $"'{value}'" : "NULL",
        DataType.ColumnDataType.Int => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.UInt => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.Long => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.Float => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.Decimal => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.Double => value != null ? $"{value}" : "NULL",
        DataType.ColumnDataType.DateTime => value != null ? $"'{value}'" : "NULL",
        DataType.ColumnDataType.Object => value != null ? $"'{value}'" : "NULL",
        DataType.ColumnDataType.Binary => value != null ? $"'{value}'" : "NULL",
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };

    public static string GetParameterStringBasedOnSqlDbType(this ColumnDataType columnDataType, string parameterName, object? value) => columnDataType switch
    {
        DataType.ColumnDataType.Char => $"{parameterName}",
        DataType.ColumnDataType.String => $"{parameterName}",
        DataType.ColumnDataType.Boolean => $"{parameterName}",
        DataType.ColumnDataType.Byte => $"{parameterName}",
        DataType.ColumnDataType.Int => $"{parameterName}",
        DataType.ColumnDataType.UInt => $"{parameterName}",
        DataType.ColumnDataType.Long => $"{parameterName}",
        DataType.ColumnDataType.Float => $"{parameterName}",
        DataType.ColumnDataType.Decimal => $"{parameterName}",
        DataType.ColumnDataType.Double => $"{parameterName}",
        DataType.ColumnDataType.DateTime => $"{parameterName}",
        DataType.ColumnDataType.Object => $"{parameterName}",
        DataType.ColumnDataType.Structure => $"{parameterName}",
        _ => throw new ArgumentOutOfRangeException(nameof(columnDataType), columnDataType, null)
    };

    public static string ParameterPhrase(this IColumnPropertyInfo columnPropertyInfo, string parameterName) => $"[{columnPropertyInfo.ColumnName}] = {parameterName}";

    public static IEnumerable<SqlParameter> ArrayParameters<TDataType>(string baseParameterName, IEnumerable<TDataType> values, ColumnDataType columnDataType, int? index = 0)
    {
        var dbType = columnDataType.SqlDbType();

        foreach (var value in values)
        {
            var parameterName = $"@{baseParameterName}_{index++}";
            var parameter = new SqlParameter(parameterName, dbType)
            {
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
