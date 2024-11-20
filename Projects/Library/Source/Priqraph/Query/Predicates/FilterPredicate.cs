using Priqraph.Contract;
using Priqraph.DataType;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class FilterPredicate<TObject> where TObject : IQueryableObject
    {
        public Expression<Func<TObject, bool>>? Expression
        {
            get;
            set;
        }

        public ReturnType ReturnType
        {
            get;
            set;
        }

        internal FilterPredicate(Expression<Func<TObject, bool>> expression)
        {
            Expression = expression;
        }

        internal FilterPredicate(ReturnType returnType)
        {
            ReturnType = returnType;
        }

        internal FilterPredicate(Expression<Func<TObject, bool>> expression, ReturnType returnType)
        {
            Expression = expression;
            ReturnType = returnType;
        }

        public static FilterPredicate<TObject> CreateFilter(Expression<Func<TObject, bool>> expression) => new(expression);
    }
}