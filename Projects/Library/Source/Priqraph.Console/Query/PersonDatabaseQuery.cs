using System.Data.SqlClient;
using Priqraph.Console.Model;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using Priqraph.Manager.Database;
using Priqraph.Query;

namespace Priqraph.Console.Query;
public class PersonDatabaseQuery : PersonQuery<DatabaseQueryOperationType, DatabaseQueryPartCollection>
{
    //Virtual member called in constructor ==> use sealed
    protected sealed override IQueryOperation<Person, DatabaseQueryOperationType, DatabaseQueryPartCollection> Operation
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

        var queryPartCollection = await Operation.RunAsync(queryObject);
        var select = queryPartCollection.GetSelectQuery(out IEnumerable<SqlParameter>? parameters);
        return select;
    }
}


