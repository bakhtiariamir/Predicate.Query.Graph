using Priqraph.Contract;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class PagePredicateBuilder : IQueryObjectPart<PagePredicateBuilder, PagePredicate>
{
    private readonly PagePredicate _pagePredicate;

    private PagePredicateBuilder(Expression<Func<PageClause>> expression)
    {
        _pagePredicate = new PagePredicate(expression);
    }

    public static PagePredicateBuilder Init(int skip, int take) => new(() => new PageClause(skip, take));

    public PagePredicateBuilder Validate() => this;

    public PagePredicate Return() => _pagePredicate;

    public Dictionary<string, string> GetQueryOptions() => new();
}

public class PageClause
{
    public int Skip
    {
        get;
    }

    public int Take
    {
        get;
    }

    public PageClause(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}
