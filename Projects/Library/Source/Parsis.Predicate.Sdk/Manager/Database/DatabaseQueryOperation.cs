using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Manager.Database;

public abstract class DatabaseQueryOperation<TObject> : QueryOperation<TObject, DatabaseQueryPartCollection<TObject>, DatabaseQueryOperationType> where TObject : IQueryableObject
{
    protected override QueryObject<TObject, DatabaseQueryOperationType>? QueryObject
    {
        get;
        set;
    }

    public override async Task<DatabaseQueryPartCollection<TObject>> RunAsync(QueryObject<TObject, DatabaseQueryOperationType> queryObject, DatabaseQueryOperationType operationType)
    {
        QueryObject = queryObject ?? throw new System.Exception("Asd");

        QueryObject = QueryObjectReducer<TObject, DatabaseQueryOperationType>.Init(queryObject).Reduce().Return();

          var validateQuery = await ValidateAsync();
        if (validateQuery)
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

        throw new System.Exception("asd"); //ToDo
    }

    protected abstract Task<DatabaseQueryPartCollection<TObject>> SelectAsync();

    protected abstract Task<DatabaseQueryPartCollection<TObject>> InsertAsync();

    protected abstract Task<DatabaseQueryPartCollection<TObject>> UpdateAsync();

    protected abstract Task<DatabaseQueryPartCollection<TObject>> DeleteAsync();
}


