using Priqraph.Contract;

namespace Priqraph.Builder.Database;

public class SqlServerQueryBuilder<TObject> : DatabaseQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly QueryContextBuilder _contextBuilder;

    private IQueryContext? _queryContext;

    private IQuery<TObject, DatabaseQueryResult>? _query;

    private SqlServerQueryBuilder(ICacheInfoCollection info)
    {
        _contextBuilder = QueryContextBuilder.Init(info);
    }

    public static SqlServerQueryBuilder<TObject> Init(ICacheInfoCollection info) => new(info);

    public async Task<SqlServerQueryBuilder<TObject>> InitContextAsync()
    {
        _queryContext = await _contextBuilder.Build();
        return this;
    }

    public Task<SqlServerQueryBuilder<TObject>> InitQueryAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asdas"); //ToDo

        _query = new SqlServerQuery<TObject>(_queryContext);
        return Task.FromResult(this);
    }

    public override Task<IQuery<TObject, DatabaseQueryResult>> BuildAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asd"); //ToDo : Exception

        if (_query == null)
            throw new System.Exception("asda"); //ToDo

        return Task.FromResult(_query);
    }
}
