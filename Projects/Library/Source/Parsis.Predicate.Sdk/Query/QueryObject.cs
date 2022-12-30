using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObject<TObject, TQueryType> where TObject : IQueryableObject where TQueryType : Enum
{
    public TQueryType QueryType
    {
        get;
        set;
    }

    public ObjectInitializing<TObject>? Insert
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

    public QueryObject(TQueryType queryType) => QueryType = queryType;
}