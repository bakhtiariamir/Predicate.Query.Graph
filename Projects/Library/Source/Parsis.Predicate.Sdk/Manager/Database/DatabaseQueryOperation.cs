using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Manager.Database;

public abstract class DatabaseQueryOperation<TObject> : QueryOperation<TObject, DatabaseQueryPartCollection, DatabaseQueryOperationType> where TObject : IQueryableObject
{
    protected override QueryObject<TObject, DatabaseQueryOperationType>? QueryObject
    {
        get;
        set;
    }

    public override async Task<DatabaseQueryPartCollection> RunAsync(QueryObject<TObject, DatabaseQueryOperationType> queryObject)
    {
        QueryObject = queryObject ?? throw new System.Exception("Asd");

        QueryObject = QueryObjectReducer<TObject, DatabaseQueryOperationType>.Init(queryObject).Reduce().Return();

        var validateQuery = await ValidateAsync();
        if (validateQuery)
        {
            return queryObject.QueryType switch {
                DatabaseQueryOperationType.Select => await SelectAsync(),
                DatabaseQueryOperationType.Insert => await InsertAsync(),
                DatabaseQueryOperationType.Update => await UpdateAsync(),
                DatabaseQueryOperationType.Delete => await DeleteAsync(),
                _ => throw new System.Exception("Error")
            };
        }

        throw new System.Exception("asd"); //ToDo
    }

    protected abstract Task<DatabaseQueryPartCollection> SelectAsync();

    protected abstract Task<DatabaseQueryPartCollection> InsertAsync();

    protected abstract Task<DatabaseQueryPartCollection> UpdateAsync();

    protected abstract Task<DatabaseQueryPartCollection> DeleteAsync();
}
