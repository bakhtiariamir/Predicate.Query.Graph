using System.ComponentModel;
using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Manager.Database;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Console.Query;
public class UserDatabaseQuery : UserQuery<DatabaseQueryOperationType, DatabaseQueryPartCollection<User>>
{
    //Virtual member called in constructor ==> use sealed
    protected sealed override IQueryOperation<User, DatabaseQueryOperationType, DatabaseQueryPartCollection<User>> Operation
    {
        get;
        init;
    }

    public UserDatabaseQuery(IDatabaseCacheInfoCollection databaseCacheInfoCollection) => Operation = new SqlQueryOperation<User>(databaseCacheInfoCollection);

    public async Task<string> SelectQueryAsync()
    {
        var queryObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.
            Init(DatabaseQueryOperationType.Select).
            SetSelecting(QueryObjectSelecting<User>.
                Init().
                Add(item => item).Add(item => new object[] { item.Id, item.IsActive, item.Person }).
                Add(item => item.Person.Status.Name)
            ).
            SetFiltering(QueryObjectFiltering<User>.
                Init(item => item.Person.Name.RightContains("ali")).
                Or(item => item.Person.Age > 123).
                And(item => item.Person.Age< 112)).
                
            Validate().Generate();

        var queryPartCollection = await Operation.RunAsync(queryObject, DatabaseQueryOperationType.Select);
        var select = queryPartCollection.GetSelectQuery();
        return select;
    }
}


