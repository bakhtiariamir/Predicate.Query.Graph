using Priqraph.DataType;
using System.Data.SqlClient;

namespace Priqraph.Contract
{
    public interface ISqlQuery<TObject, in TEnum> : IQuery<TObject, TEnum> 
        where TObject : IQueryableObject
        where TEnum : struct, IConvertible
    {
        DatabaseQueryOperationType DatabaseQueryOperationType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Base query object for Database --> SqlServer query.
    /// SqlParameter is Type of parameter that use to generate sql server query.
    /// </summary>
    /// <seealso cref="Priqraph.Contract.IObjectQuery&lt;System.Data.SqlClient.SqlParameter&gt;" />
    public interface ISqlQuery : IObjectQuery<SqlParameter>
    {
    }
}