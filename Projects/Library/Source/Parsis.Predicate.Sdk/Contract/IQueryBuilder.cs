namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryBuilder<TObject, TQueryType, TResult> where TObject : class where TQueryType : Enum
{
    Task<IQuery<TObject, TQueryType, TResult>> BuildAsync();
}
