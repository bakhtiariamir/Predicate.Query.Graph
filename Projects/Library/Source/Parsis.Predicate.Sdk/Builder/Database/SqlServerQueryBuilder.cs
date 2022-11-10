using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQueryBuilder<TObject> : DatabaseQueryBuilder<TObject> where TObject : class
{
    private readonly DatabaseQueryContextBuilder<TObject> _contextBuilder;

    private IQueryContext<TObject>? _queryContext;

    private IQuery<TObject, DatabaseQueryPartCollection>? _query;

    public SqlServerQueryBuilder(IDatabaseCacheObjectInfo<TObject> info)
    {
        var cacheObjectInfo = info.GetLastObjectInfo();
        _contextBuilder = new DatabaseQueryContextBuilder<TObject>(cacheObjectInfo);
    }

    public static Task<SqlServerQueryBuilder<TObject>> Build(IDatabaseCacheObjectInfo<TObject> info) => Task.FromResult(new SqlServerQueryBuilder<TObject>(info));

    public async Task<SqlServerQueryBuilder<TObject>> InitContext()
    {
        _queryContext = await _contextBuilder.Build();
        return this;
    }

    public override Task<IQuery<TObject, DatabaseQueryPartCollection>> Build(QueryObject<TObject> query)
    {
        if (_queryContext == null)
            throw new System.Exception("asd");

        _query = new SqlServerQuery<TObject>(_queryContext, query);
        return Task.FromResult(_query);
    }
}