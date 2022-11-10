using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObject<TObject> where TObject : class
{
    public QueryType QueryType
    {
        get;
        set;
    }

    public IList<QueryColumn<TObject>>? Columns
    {
        get;
        set;
    }

    public IList<FilterPredicate<TObject>>? Filters
    {
        get;
        set;
    } = new List<FilterPredicate<TObject>>();

    public IList<SortPredicate<TObject>> Sorts
    {
        get;
        set;
    } = new List<SortPredicate<TObject>>();

    public PageSetting<TObject>? Paging
    {
        get;
        set;
    }

    public IList<GroupPredicate<TObject>> Groups
    {
        get;
        set;
    } = new List<GroupPredicate<TObject>>();

    public QueryObject(QueryType queryType) => QueryType = queryType;
}