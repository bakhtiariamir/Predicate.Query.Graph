using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Builder.Database;
public abstract class DatabaseQuery<TObject> : BaseQuery<TObject>, IDatabaseQuery<TObject> where TObject : class
{
    protected abstract DatabaseQueryContext<TObject> DatabaseQueryContext
    {
        get;
    }

    public abstract Task GenerateColumn();

    public abstract Task GenerateWhereClause();

    public abstract Task GeneratePagingClause();

    public abstract Task GenerateOrderByClause();

    public abstract Task GenerateJoinClause();

    public abstract Task GenerateGroupByClause();
}

