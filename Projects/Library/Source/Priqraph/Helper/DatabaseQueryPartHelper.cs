using Priqraph.Builder.Database;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using System.Data.SqlClient;

namespace Priqraph.Helper;

public static class DatabaseQueryPartHelper
{
    public static IEnumerable<SqlParameter> SelectParameters(this DatabaseQueryResult queryParts)
    {
        if (queryParts == null) throw new NotSupported(ExceptionCode.DatabaseQueryGenerator);

        var sqlParameters = FilterQueryFragment.GetParameters(queryParts.WhereClause?.Parameter, queryParts.WhereClause?.QuerySetting).ToArray();

        var pageParameters = PageQueryFragment.GetParameters(queryParts.Paging?.Parameter)?.ToArray();


        var parameters = sqlParameters.Concat(pageParameters ?? Array.Empty<SqlParameter>());

        foreach (var parameter in parameters)
            yield return parameter;
    }
}
