using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryContextBuilder<TObject> : IQueryContextBuilder<TObject> where TObject : class
{
    public abstract Task<IQueryContext<TObject>> Build();

}
