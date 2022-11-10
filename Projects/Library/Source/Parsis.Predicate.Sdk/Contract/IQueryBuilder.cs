using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryBuilder<TObject, TResult> where TObject : class
{
    Task<IQuery<TObject, TResult>> Build(QueryObject<TObject> query);
}
