using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class SqlServerQueryBuilder<TObject> : DatabaseQueryBuilder<TObject> where TObject : IQueryableObject
{
    private readonly DatabaseQueryContextBuilder _contextBuilder;

    private IQueryContext? _queryContext;

    private IQuery<TObject, DatabaseQueryPartCollection>? _query;

    private SqlServerQueryBuilder(ICacheInfoCollection info) => _contextBuilder = new DatabaseQueryContextBuilder(info);

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

    public override Task<IQuery<TObject, DatabaseQueryPartCollection>> BuildAsync()
    {
        if (_queryContext == null)
            throw new System.Exception("asd"); //ToDo : Exception

        if (_query == null)
            throw new System.Exception("asda"); //ToDo

        return Task.FromResult(_query);
    }
}
