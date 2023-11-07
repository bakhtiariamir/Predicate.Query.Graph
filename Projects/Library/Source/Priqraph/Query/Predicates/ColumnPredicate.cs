using System;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class ColumnPredicate<TObject>
    {
        public Expression<Func<TObject, object>>? Expression
        {
            get;
        }

        public Expression<Func<TObject, IEnumerable<object>>>? Expressions
        {
            get;
        }

        public ColumnPredicate(Expression<Func<TObject, object>>? expression)
        {
            Expression = expression;
        }

        public ColumnPredicate(Expression<Func<TObject, IEnumerable<object>>>? expressions)
        {
            Expressions = expressions;
        }
    }
}