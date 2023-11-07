using Priqraph.DataType;
using System;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class JoinPredicate
    {
        public Expression PropertyExpression
        {
            get;
        }

        public JoinType Type
        {
            get;
        }

        public int Order
        {
            get;
        }

        public JoinPredicate(Expression propertyExpression, JoinType type, int order)
        {
            PropertyExpression = propertyExpression;
            Type = type;
            Order = order;
        }
    }
}