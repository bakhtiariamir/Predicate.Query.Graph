using Parsis.Predicate.Sdk.Builder.Database;

namespace Parsis.Predicate.Sdk.Manager.Database;

public class SqlQueryOperation<TObject> : DatabaseQueryOperation<TObject> where TObject : class
{
    public override Task<DatabaseQueryPartCollection> SelectAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<DatabaseQueryPartCollection> InsertAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<DatabaseQueryPartCollection> UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<DatabaseQueryPartCollection> DeleteAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<DatabaseQueryPartCollection> GetQueryPartsAsync()
    {
        throw new NotImplementedException();
    }
}
