using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Sql.Builder;

namespace Priqraph.Sql.Manager;

public class SqlServerQueryBuilder<TObject>(ICacheInfoCollection cacheInfoCollection) : ISqlServerQueryBuilder<TObject>
    where TObject : IQueryableObject
{
    public IQueryObject<TObject, ISqlQuery<TObject, DatabaseQueryOperationType>, DatabaseQueryResult, DatabaseQueryOperationType> Build(bool isObjectQuery = true)
    {
        throw new NotImplementedException();
    }
}
