using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class BaseQuery<TObject> : IQuery<TObject> where TObject : class
{
    public abstract Task Generate();
}

