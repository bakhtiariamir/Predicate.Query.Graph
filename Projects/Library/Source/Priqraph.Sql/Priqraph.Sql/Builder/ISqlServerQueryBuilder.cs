using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Sql.Builder;

public interface ISqlServerQueryBuilder<TObject> : IQueryBuilder<TObject, ISqlQuery<TObject, DatabaseQueryOperationType>, DatabaseQueryResult, DatabaseQueryOperationType> 
    where TObject : IQueryableObject
{
}