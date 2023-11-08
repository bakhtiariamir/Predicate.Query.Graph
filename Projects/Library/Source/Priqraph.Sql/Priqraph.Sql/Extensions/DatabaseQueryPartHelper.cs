using Priqraph.Builder.Database;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using System.Data.SqlClient;
using Priqraph.Sql.Generator;

namespace Priqraph.Sql.Extensions;
public static class DatabaseQueryPartHelper
{
    public static IEnumerable<SqlParameter> SelectParameters(this DatabaseQueryResult queryParts)
    {
        if (queryParts == null) throw new NotSupported(ExceptionCode.DatabaseQueryGenerator);

        var sqlParameters = FilterQueryFragment.GetParameters(queryParts.FilterFragment?.Parameter, queryParts.FilterFragment?.QuerySetting).ToArray();

        var pageParameters = PageQueryFragment.GetParameters(queryParts.PageFragment?.Parameter)?.ToArray();


        var parameters = sqlParameters.Concat(pageParameters ?? Array.Empty<SqlParameter>());

        foreach (var parameter in parameters)
            yield return parameter;
    }
}
