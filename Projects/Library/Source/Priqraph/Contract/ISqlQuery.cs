using System.Data.SqlClient;

namespace Priqraph.Contract
{
    /// <summary>
    /// Base query object for Database --> SqlServer query.
    /// SqlParameter is Type of parameter that use to generate sql server query.
    /// </summary>
    /// <seealso cref="Priqraph.Contract.IObjectQuery&lt;System.Data.SqlClient.SqlParameter&gt;" />
    public interface ISqlQuery : IObjectQuery<SqlParameter>
    {
    }
}
