using Priqraph.Builder.Database;
using Priqraph.Contract;

namespace Priqraph.Sql.Manager;

public class SqlServerQueryBuilder<TObject> : ISqlServerQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly ICacheInfoCollection _cacheInfoCollection;

    public SqlServerQueryBuilder(ICacheInfoCollection cacheInfoCollection)
    {
        _cacheInfoCollection = cacheInfoCollection;
    }

    public IQueryObject<TObject, DatabaseQueryResult> Build(bool isObjectQuery = true)
    {
	    return isObjectQuery  ? new SqlServerQueryObject<TObject>(_cacheInfoCollection) : new SqlServerQueryableQueryObject<TObject>(_cacheInfoCollection);
    }
}
