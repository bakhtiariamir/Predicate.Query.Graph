using Priqraph.Contract;

namespace Priqraph.Sql.Manager;

public class SqlServerQueryBuilder<TObject> : ISqlServerQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly ICacheInfoCollection _cacheInfoCollection;

    public SqlServerQueryBuilder(ICacheInfoCollection cacheInfoCollection)
    {
        _cacheInfoCollection = cacheInfoCollection;
    }

    public ISqlServerQuery<TObject> Build() => new SqlServerQuery<TObject>(_cacheInfoCollection);
}
