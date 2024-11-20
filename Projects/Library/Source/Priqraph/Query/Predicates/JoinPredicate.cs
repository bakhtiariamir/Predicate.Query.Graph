using Priqraph.DataType;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates
{
    public class JoinPredicate(Expression propertyExpression, JoinType type, int order)
    {
        public Expression PropertyExpression
        {
            get;
        } = propertyExpression;

        public JoinType Type
        {
            get;
        } = type;

        public int Order
        {
            get;
        } = order;
    }
}