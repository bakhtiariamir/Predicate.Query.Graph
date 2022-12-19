using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQueryBuilder<TObject> : DatabaseQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly DatabaseQueryContextBuilder<TObject> _contextBuilder;

    private IQueryContext? _queryContext;

    private IQuery<TObject, DatabaseQueryOperationType, DatabaseQueryPartCollection<TObject>>? _query;

    private SqlServerQueryBuilder(IDatabaseCacheInfoCollection info)
    {
        _contextBuilder = new DatabaseQueryContextBuilder<TObject>(info);
    }

    public static SqlServerQueryBuilder<TObject> Init(IDatabaseCacheInfoCollection info) => new(info);

    public async Task<SqlServerQueryBuilder<TObject>> InitContextAsync()
    {
        _queryContext = await _contextBuilder.Build();
        return this;
    }

    public Task<SqlServerQueryBuilder<TObject>> InitQueryAsync(DatabaseQueryOperationType queryType)
    {
        if (_queryContext == null)
            throw new System.Exception("asdas"); //ToDo

        _query = new SqlServerQuery<TObject>(_queryContext, queryType);
        return Task.FromResult(this);
    }

    public override Task<IQuery<TObject, DatabaseQueryOperationType, DatabaseQueryPartCollection<TObject>>> BuildAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asd"); //ToDo : Exception

        if (_query == null)
            throw new System.Exception("asda"); //ToDo

        return Task.FromResult(_query);
    }
}