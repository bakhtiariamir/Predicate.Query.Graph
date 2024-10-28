using Priqraph.Builder.Database;
using Priqraph.Contract;

namespace Priqraph.Sql.Builder;

public interface ISqlServerQueryableQueryObject<TObject> : IQueryObject<TObject, ISqlQuery<TObject>, DatabaseQueryResult> 
    where TObject : IQueryableObject
{

}