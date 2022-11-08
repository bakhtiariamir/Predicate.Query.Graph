using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Info;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQueryContext<TObject> : DatabaseQueryContext<TObject> where TObject : class
{
    public SqlServerObjectInfo<TObject> SqlServerObjectInfo
    {
        get;
        set;
    }
    public IDictionary<string, string> QueryParts
    {
        get;
        set;
    } = new Dictionary<string, string>();


    public SqlServerQueryContext(SqlServerObjectInfo<TObject> sqlServerObjectInfo)
    {
        SqlServerObjectInfo = sqlServerObjectInfo;
    }

}