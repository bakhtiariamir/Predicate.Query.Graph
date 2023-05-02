using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectPaging : IQueryObjectPart<QueryObjectPaging, PagePredicate>
{
    private PagePredicate _pagePredicate;

    private QueryObjectPaging(Expression<Func<Page>> expression) => _pagePredicate = new PagePredicate(expression);

    public static QueryObjectPaging Init(int skip, int take) => new(() => new Page(skip, take));

    public QueryObjectPaging Validate() => this;

    public PagePredicate Return() => _pagePredicate;

    public Dictionary<string, string> GetQueryOptions() => new();
}

public class PagePredicate
{
    public PagePredicate(Expression<Func<Page>> predicate)
    {
        Predicate = predicate;
    }

    public Expression<Func<Page>> Predicate
    {
        get;
    }
}

public class Page
{
    public int Skip
    {
        get;
    }

    public int Take
    {
        get;
    }

    public Page(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}
