using Priqraph.Query.Builders;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class PagePredicate(Expression<Func<PageClause>> predicate)
    {
        public Expression<Func<PageClause>> Predicate
        {
            get;
        } = predicate;
    }
}
