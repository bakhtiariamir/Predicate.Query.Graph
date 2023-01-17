namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryBuilder<TObject, TQueryType, TResult> where TObject : IQueryableObject
    where TQueryType : Enum
{
    Task<IQuery<TObject, TQueryType, TResult>> BuildAsync();
}
