using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectPaging : IQueryObjectPart<QueryObjectPaging, PagePredicate>
{
    private PagePredicate _pagePredicate;

    private QueryObjectPaging(Expression<Func<Page>> expression) => _pagePredicate = new PagePredicate(expression);

    public static QueryObjectPaging Init(int pageSize, int pageRows) => new(() => new Page(pageSize, pageRows));

    public QueryObjectPaging Validate() => this;

    public PagePredicate Return() => _pagePredicate;
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
    public int PageNumber
    {
        get;
    }

    public int PageRows
    {
        get;
    }

    public Page(int pageNumber, int pageRows)
    {
        PageNumber = pageNumber;
        PageRows = pageRows;
    }
}
