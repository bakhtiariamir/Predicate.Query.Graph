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

    public IQuery<TObject, DatabaseQueryResult> Build(bool isObjectQuery = true)
    {
	    return isObjectQuery  ? new SqlServerQuery<TObject>(_cacheInfoCollection) : new SqlServerQueryableQuery<TObject>(_cacheInfoCollection);
    }
}
