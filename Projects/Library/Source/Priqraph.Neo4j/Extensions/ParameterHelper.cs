using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Neo4j.Extensions;

public static class ParameterHelper
{
    public static string ParameterPhrase(this IColumnPropertyInfo columnPropertyInfo, string parameterName) => $"[{columnPropertyInfo.ColumnName}] = {parameterName}";

    public static IEnumerable<Neo4JParameter> ArrayParameters<TDataType>(
        string baseParameterName, 
        IEnumerable<TDataType> values, 
        ColumnDataType columnDataType, 
        int? index = 0)
    {
        foreach (var value in values)
        {
            var parameterName = $"@{baseParameterName}_{index++}";
            var parameter = new Neo4JParameter(parameterName, value)
            {
                DataType =  ColumnDataType.Object
            };
            yield return parameter;
        }
    }
    
    public static string ArrayParameterNames(
        this IColumnPropertyInfo column, 
        string baseParameterName, 
        int valueCount, 
        int? index = 0)
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