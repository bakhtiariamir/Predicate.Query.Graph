using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder; 
public abstract class QueryGeneratorFactory<TObject> : IQueryGeneratorFactory<TObject> where TObject : class
{
    public abstract IQuery<TObject> Init(ObjectQuery<TObject> query);
}

