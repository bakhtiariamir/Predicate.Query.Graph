using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;

public abstract class QueryContextBuilder : IQueryContextBuilder
{
    public abstract Task<IQueryContext> Build();
}
