using Parsis.Predicate.Sdk.Builder;

namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryContextBuilder<TObject> where TObject : class
{
    Task<IQueryContext<TObject>> Build();
}
