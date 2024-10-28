using Priqraph.Builder.Database;
using Priqraph.Contract;

namespace Priqraph.Sql.Builder;


public interface ISqlServerQueryObject<TObject, TEnum> : IQueryObject<TObject, ISqlQuery<TObject, TEnum>, DatabaseQueryResult, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}