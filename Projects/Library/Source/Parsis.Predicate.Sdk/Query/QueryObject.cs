using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObject<TObject> where TObject : IQueryableObject
{
    public QueryOperationType QueryOperationType
    {
        get;
        set;
    }

    public ObjectCommand<TObject>? Command
    {
        get;
        set;
    }

    public ICollection<QueryColumn<TObject>>? Columns
    {
        get;
        set;
    }

    public ICollection<JoinPredicate>? Joins
    {
        get;
        set;
    }

    public FilterPredicate<TObject>? Filters
    {
        get;
        set;
    }

    public ICollection<SortPredicate<TObject>>? Sorts
    {
        get;
        set;
    }

    public PagePredicate? Paging
    {
        get;
        set;
    }

    private QueryObject(QueryOperationType queryOperationType) => QueryOperationType = queryOperationType;

    public static QueryObject<TObject> Init(QueryOperationType queryOperationType) => new(queryOperationType);
}
