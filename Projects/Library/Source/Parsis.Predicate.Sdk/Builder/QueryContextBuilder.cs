using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class QueryContextBuilder<TObject> : IQueryContextBuilder where TObject : class
{
    public abstract Task<IQueryContext> Build();

}
