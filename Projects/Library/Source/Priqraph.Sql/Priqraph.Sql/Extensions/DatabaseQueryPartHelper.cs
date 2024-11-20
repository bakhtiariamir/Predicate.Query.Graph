using Priqraph.Builder.Database;
using Priqraph.Exception;
using Microsoft.Data.SqlClient;
using Priqraph.Sql.Generator;

namespace Priqraph.Sql.Extensions;
public static class DatabaseQueryPartHelper
{
    public static IEnumerable<SqlParameter> SelectParameters(this DatabaseQueryResult queryParts)
    {
        if (queryParts == null) throw new NotSupportedOperationException(ExceptionCode.DatabaseQueryGenerator);

        var sqlParameters = FilterQueryFragment.GetParameters(queryParts.FilterFragment?.Parameter, queryParts.FilterFragment?.QuerySetting!)?.ToArray();

        var pageParameters = PageQueryFragment.GetParameters(queryParts.PageFragment?.Parameter)?.ToArray();


        var parameters = sqlParameters?.Concat(pageParameters ?? []);

        if (parameters is null)
            yield break;
        
        foreach (var parameter in parameters)
            yield return parameter;
    }
}
