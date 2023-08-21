using System.Data.SqlClient;
using Priqraph.Console.Model;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using Priqraph.Manager.Database;
using Priqraph.Query;

namespace Priqraph.Console.Query;
public class UserDatabaseQuery : UserQuery<DatabaseQueryOperationType, DatabaseQueryPartCollection>
{
    //Virtual member called in constructor ==> use sealed
    protected sealed override IQueryOperation<User, DatabaseQueryOperationType, DatabaseQueryPartCollection> Operation
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

                Add(item => item.Person).
                Add(item => item.Person.Status)
            ).
            SetFiltering(QueryObjectFiltering<User>.
                Init(item => item.Username != null)).
            SetSorting(QueryObjectSorting<User>.Init().Add(item => item.IsActive, DirectionType.Desc)).
            SetPaging(QueryObjectPaging.Init(10, 1)).
            Validate().Generate();

        var queryPartCollection = await Operation.RunAsync(queryObject);
        var select = queryPartCollection.GetSelectQuery(out IEnumerable<SqlParameter>? parameters);
        return select;
    }

    public async Task<string> InsertQueryAsync(User user)
    {
        var insertObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Insert).SetCommand(QueryObjectCommand<User>.InitInsert(CommandValueType.Record).Add(_ => user)).Validate().Generate();
        var insertQuery = await Operation.RunAsync(insertObject);
        var insert = insertQuery.GetCommandQuery(out var paramters);

        return insert;
    }

    public async Task<string> BulkInsertQueryAsync(IEnumerable<User> user)
    {
        var insertObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Insert).SetCommand(QueryObjectCommand<User>.InitInsert(CommandValueType.Record).AddRange(_ => user)).Validate().Generate();
        var insertQuery = await Operation.RunAsync(insertObject);
        var insert = insertQuery.GetCommandQuery(out var paramters);

        return insert;
    }

    public async Task<string> UpdateCommandAsync(User user)
    {
        var updateObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Update).SetCommand(QueryObjectCommand<User>.InitUpdate(CommandValueType.Record).Add(_ => user)).Validate().Generate();
        var updateQuery = await Operation.RunAsync(updateObject);
        var update = updateQuery.GetCommandQuery(out var paramters);

        return update;
    }


    public async Task<string> BulkUpdateCommandAsync(IEnumerable<User> users)
    {
        var updateObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Update).SetCommand(QueryObjectCommand<User>.InitUpdate(CommandValueType.Record).AddRange(_ => users)).Validate().Generate();
        var updateQuery = await Operation.RunAsync(updateObject);
        var update = updateQuery.GetCommandQuery(out var paramters);

        return update;
    }


    public async Task<string> DeleteCommandAsync(User user)
    {
        var deleteObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Delete).SetCommand(QueryObjectCommand<User>.InitDelete(CommandValueType.Record).Add(_ => user)).Validate().Generate();
        var deleteQuery = await Operation.RunAsync(deleteObject);
        var delete = deleteQuery.GetCommandQuery(out var paramters);

        return delete;
    }


    public async Task<string> BulkDeleteCommandAsync(IEnumerable<User> users)
    {
        var deleteObject = QueryObjectBuilder<User, DatabaseQueryOperationType>.Init(DatabaseQueryOperationType.Delete).SetCommand(QueryObjectCommand<User>.InitDelete(CommandValueType.Record).AddRange(_ => users)).Validate().Generate();
        var deleteQuery = await Operation.RunAsync(deleteObject);
        var delete = deleteQuery.GetCommandQuery(out var paramters);

        return delete;
    }


}




