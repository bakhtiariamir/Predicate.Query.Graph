using Priqraph.Contract;
using Priqraph.DataType;
using System;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class SortPredicate<TObject> where TObject : IQueryableObject
    {
        public Expression<Func<TObject, object>>? Expression
        {
            get;
        }

        public DirectionType DirectionType
        {
            get;
        }

        internal SortPredicate(Expression<Func<TObject, object>> expression, DirectionType directionType)
        {
            Expression = expression;
            DirectionType = directionType;
        }

        public static SortPredicate<TObject> CreateSorPredicate(Expression<Func<TObject, object>> expression, DirectionType directionType) => new(expression, directionType);
    }
}
