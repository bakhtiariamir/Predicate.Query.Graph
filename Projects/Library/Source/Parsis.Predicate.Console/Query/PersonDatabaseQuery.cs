using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Manager.Database;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Console.Query;
public class PersonDatabaseQuery : PersonQuery<DatabaseQueryOperationType, DatabaseQueryPartCollection<Person>>
{
    //Virtual member called in constructor ==> use sealed
    protected sealed override IQueryOperation<Person, DatabaseQueryOperationType, DatabaseQueryPartCollection<Person>> Operation
    {
        get;
        init;
    }

    public PersonDatabaseQuery(IDatabaseCacheInfoCollection databaseCacheInfoCollection) => Operation = new SqlQueryOperation<Person>(databaseCacheInfoCollection);

    public async Task<string> SelectQueryAsync()
    {
        var queryObject = QueryObjectBuilder<Person, DatabaseQueryOperationType>.
            Init(DatabaseQueryOperationType.Select).
            SetSelecting(QueryObjectSelecting<Person>.
                Init().
                Add(item => new object[]{item.Id, item.Name, item.Age}).
                Add(item => item.Name).
                Add(item => item.Age)
            ).
            SetFiltering(QueryObjectFiltering<Person>.
                Init(item => item.Age > 123).
                And(item => item.Age< 112)).
            Validate().Generate();

        var queryPartCollection = await Operation.RunAsync(queryObject, DatabaseQueryOperationType.Select);
        var select = queryPartCollection.GetSelectQuery();
        return select;
    }
}


