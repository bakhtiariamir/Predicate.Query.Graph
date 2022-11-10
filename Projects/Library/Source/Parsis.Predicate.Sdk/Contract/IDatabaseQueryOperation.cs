using Parsis.Predicate.Sdk.Builder.Database;

namespace Parsis.Predicate.Sdk.Contract;
public interface IDatabaseQueryOperation<TObject> where TObject : class
{
    Task<DatabaseQueryPartCollection> SelectAsync();

    Task<DatabaseQueryPartCollection> InsertAsync();

    Task<DatabaseQueryPartCollection> UpdateAsync();

    Task<DatabaseQueryPartCollection> DeleteAsync();

    Task<DatabaseQueryPartCollection> GetQueryPartsAsync();


}
