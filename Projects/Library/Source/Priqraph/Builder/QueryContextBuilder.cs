using Priqraph.Contract;

namespace Priqraph.Builder;

public abstract class QueryContextBuilder : IQueryContextBuilder
{
    public abstract Task<IQueryContext> Build();
}
