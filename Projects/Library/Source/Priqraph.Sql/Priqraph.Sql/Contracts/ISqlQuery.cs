using Priqraph.Contract;
using System.Data.SqlClient;

namespace Priqraph.Sql.Contracts;
public interface ISqlQuery : IObjectQuery<SqlParameter>
{
}

