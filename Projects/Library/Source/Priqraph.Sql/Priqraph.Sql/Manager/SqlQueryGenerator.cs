using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Sql.Extensions;
using System.Data.SqlClient;

namespace Priqraph.Sql.Manager;
public class SqlQueryGenerator : IObjectQueryGenerator<SqlParameter, SqlObjectQuery, DatabaseQueryResult>
{
    public SqlObjectQuery? GenerateResult(DatabaseQueryOperationType operationType, DatabaseQueryResult query)
    {
        var phrase = string.Empty;
        ICollection<SqlParameter>? parameters = null;
        switch (operationType)
        {
            case DatabaseQueryOperationType.GetData:
                phrase = query.Select(out parameters);
                break;
            case DatabaseQueryOperationType.Add:
            case DatabaseQueryOperationType.Edit:
            case DatabaseQueryOperationType.Remove:
            case DatabaseQueryOperationType.Merge:
                phrase = query.Command(out parameters);
                break;
        }

        return new SqlObjectQuery(parameters, phrase);
    }
}