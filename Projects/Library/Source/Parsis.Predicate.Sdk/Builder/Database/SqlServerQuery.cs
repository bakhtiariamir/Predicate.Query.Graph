using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQuery<TObject> : DatabaseQuery<TObject> where TObject : class
{
    private readonly DatabaseQueryContext<TObject> _context;

    public override DatabaseProviderType ProviderType => DatabaseProviderType.SqlServer;

    protected override DatabaseQueryPartCollection QueryPartCollection
    {
        get;
        set;
    } = new();


    public SqlServerQuery(IQueryContext<TObject> context, QueryObject<TObject> query) : base(query)
    {
        _context = (DatabaseQueryContext<TObject>)context;
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

    public override Task<DatabaseQueryPartCollection> Build()
    {
        Task.WaitAll(new[]
        {
            GenerateColumn(), GenerateWhereClause(), GenerateOrderByClause(), GenerateJoinClause()
        });

        return Task.FromResult(QueryPartCollection);
    }
}
