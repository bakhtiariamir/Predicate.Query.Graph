namespace Priqraph.Contract;

public interface IQueryContextBuilder
{
    Task<IQueryContext> BuildAsync();
}
