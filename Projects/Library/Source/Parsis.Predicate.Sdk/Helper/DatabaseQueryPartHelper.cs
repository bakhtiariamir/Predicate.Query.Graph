using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using System.Data;
using System.Data.SqlClient;
using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseQueryPartHelper
{

    public static IEnumerable<SqlParameter> SelectParameters(this DatabaseQueryPartCollection queryParts) 
    {
        if (queryParts == null) throw new Exception.NotSupported(ExceptionCode.DatabaseQueryGenerator);

        var sqlParameters = DatabaseWhereClauseQueryPart.GetParameters(queryParts.WhereClause?.Parameter);

        foreach (var parameter in sqlParameters)
            yield return parameter;
    }
}
