namespace Parsis.Predicate.Sdk.Contract;

public interface IQuery<TObject> where TObject : class
{
    Task Generate();
}
