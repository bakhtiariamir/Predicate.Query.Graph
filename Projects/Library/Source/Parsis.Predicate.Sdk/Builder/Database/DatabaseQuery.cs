using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Builder.Database;
public abstract class DatabaseQuery<TObject> : Query<TObject, DatabaseQueryPartCollection>, IDatabaseQuery<TObject> where TObject : class
{
    public abstract DatabaseProviderType ProviderType
    {
        get;
    }
    protected DatabaseQuery(QueryObject<TObject> objectQuery) : base(objectQuery)
    {
    }

    protected abstract DatabaseQueryPartCollection QueryPartCollection
    {
        get;
        set;
    }

    public abstract Task GenerateColumn();

    public abstract Task GenerateWhereClause();

    public abstract Task GeneratePagingClause();

    public abstract Task GenerateOrderByClause();

    public abstract Task GenerateJoinClause();

    public abstract Task GenerateGroupByClause();
}

