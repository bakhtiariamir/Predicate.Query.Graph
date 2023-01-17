using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Database;
using System.Data.SqlClient;

namespace Parsis.Predicate.Sdk.Helper;

public static class DatabaseQueryPartHelper
{
    public static IEnumerable<SqlParameter> SelectParameters(this DatabaseQueryPartCollection queryParts)
    {
        if (queryParts == null) throw new NotSupported(ExceptionCode.DatabaseQueryGenerator);

        var sqlParameters = DatabaseWhereClauseQueryPart.GetParameters(queryParts.WhereClause?.Parameter);

        foreach (var parameter in sqlParameters)
            yield return parameter;
    }
}
