using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder;
public abstract class OracleQueryGenerator<TObject> : QueryGeneratorFactory<TObject> where TObject : class
{
    public override IQuery<TObject> Init(ObjectQuery<TObject> query)
    {
        throw new NotImplementedException();
    }
}

