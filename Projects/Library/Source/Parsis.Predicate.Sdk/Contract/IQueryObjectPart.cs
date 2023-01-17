namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryObjectPart<out TQueryPart, out TResult>
{
    TQueryPart Validate();

    TResult Return();
}
