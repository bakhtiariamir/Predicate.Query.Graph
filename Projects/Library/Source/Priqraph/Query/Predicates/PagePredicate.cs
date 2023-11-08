using Priqraph.Query.Builders;
using System;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class PagePredicate
    {
        public PagePredicate(Expression<Func<PageClause>> predicate)
        {
            Predicate = predicate;
        }

        public Expression<Func<PageClause>> Predicate
        {
            get;
        }
    }
}
