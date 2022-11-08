using Microsoft.Extensions.Caching.Memory;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQuery<TObject> : DatabaseQuery<TObject> where TObject : class
{
    private readonly IMemoryCache _memoryCache;
    protected override DatabaseQueryContext<TObject> DatabaseQueryContext => DatabaseQueryContextHelper.GenerateSqlServerQueryContext<TObject>(_memoryCache);

    public SqlServerQuery(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public override Task GenerateColumn()
    {
        throw new NotImplementedException();
    }

    public override Task GenerateWhereClause()
    {
        throw new NotImplementedException();
    }

    public override Task GeneratePagingClause()
    {
        throw new NotImplementedException();
    }

    public override Task GenerateOrderByClause()
    {
        throw new NotImplementedException();
    }

    public override Task GenerateJoinClause()
    {
        throw new NotImplementedException();
    }

    public override Task GenerateGroupByClause()
    {
        throw new NotImplementedException();
    }

    public override Task Generate()
    {
        Task.WaitAll(new[]
        {
            GenerateColumn(), GenerateWhereClause(), GenerateOrderByClause(), GenerateJoinClause()
        });

        return Task.CompletedTask;
    }
}
