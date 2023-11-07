using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using System.Data.SqlClient;

namespace Priqraph.Manager.Result.Database
{
    public class SqlObjectQueryGenerator : IObjectQueryGenerator<SqlParameter, SqlObjectQuery, DatabaseQueryResult>
    {
        public SqlObjectQuery? GenerateResult(QueryOperationType operationType, DatabaseQueryResult query)
        {
            var phrase = string.Empty;
            ICollection<SqlParameter>? parameters = null;
            switch (operationType)
            {
                case QueryOperationType.GetData:
                    phrase = query.Select(out parameters);
                    break;
                case QueryOperationType.Add:
                case QueryOperationType.Edit:
                case QueryOperationType.Remove:
                case QueryOperationType.Merge:
                    phrase = query.Command(out parameters);
                    break;
            }

            return new SqlObjectQuery(parameters, phrase);
        }
    }
}
