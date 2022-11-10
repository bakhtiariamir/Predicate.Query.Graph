namespace Parsis.Predicate.Sdk.Contract;
public interface IQueryObjectPart<out TQueryPart>
{
    TQueryPart Validation();
}
