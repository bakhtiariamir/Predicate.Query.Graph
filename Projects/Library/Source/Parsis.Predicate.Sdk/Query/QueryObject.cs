namespace Parsis.Predicate.Sdk.Query;
public class QueryObject<TObject, TQueryType> where TObject : class where TQueryType : Enum
{
    public TQueryType QueryType
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

    public PageSetting<TObject>? Paging
    {
        get;
        set;
    }

    public ICollection<GroupPredicate<TObject>>? Groups
    {
        get;
        set;
    }

    public QueryObject(TQueryType queryType) => QueryType = queryType;
}