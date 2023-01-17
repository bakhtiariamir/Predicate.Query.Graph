namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryContextBuilder
{
    Task<IQueryContext> Build();
}
