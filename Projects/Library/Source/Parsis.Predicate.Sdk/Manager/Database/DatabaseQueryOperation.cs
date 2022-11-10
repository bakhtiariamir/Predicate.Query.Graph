using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Manager.Database;

public abstract class DatabaseQueryOperation<TObject> : QueryOperation<TObject, DatabaseQueryPartCollection, DatabaseQueryOperationType>, IDatabaseQueryOperation<TObject> where TObject : class
{
    public override async Task<DatabaseQueryPartCollection> RunAsync(DatabaseQueryOperationType operationType)
    {
        return operationType switch
        {
            DatabaseQueryOperationType.Select => await SelectAsync(),
            DatabaseQueryOperationType.Insert => await InsertAsync(),
            DatabaseQueryOperationType.Update => await UpdateAsync(),
            DatabaseQueryOperationType.Delete => await DeleteAsync(),
            _ => throw new System.Exception("Error")
        };
    }

    public abstract Task<DatabaseQueryPartCollection> SelectAsync();

    public abstract Task<DatabaseQueryPartCollection> InsertAsync();

    public abstract Task<DatabaseQueryPartCollection> UpdateAsync();

    public abstract Task<DatabaseQueryPartCollection> DeleteAsync();

    public abstract Task<DatabaseQueryPartCollection> GetQueryPartsAsync();
}


