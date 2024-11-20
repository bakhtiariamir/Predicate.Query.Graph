using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using Priqraph.Query.Reduce;
using System.Linq.Expressions;

namespace Priqraph.Query.Reduce;

internal class ColumnPredicateReducer<TObject,TQueryObject, TEnum> : PredicateReducer<TObject, TQueryObject, TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible  
{
    public override TQueryObject Reduce(TQueryObject query, ReduceType type) => base.Visit(query, type);

    protected override TQueryObject Generate(TQueryObject query)
    {
        return query;
    }

    protected override TQueryObject Decrease(TQueryObject query)
    {
        return query;
    }

    protected override TQueryObject Merge(TQueryObject query)
    {
        if (query.ColumnPredicates == null || query.ColumnPredicates.Count == 1) return query;
        var expressionParameters = query.ColumnPredicates.SelectMany(item => item.Expression?.Parameters ?? Enumerable.Empty<ParameterExpression>()).FirstOrDefault() ?? Expression.Parameter(typeof(TObject));

        var bodies = new List<Expression>();
        foreach (var column in query.ColumnPredicates)
            if (column.Expression != null)
                switch (column.Expression.Body.NodeType)
                {
                    case ExpressionType.Convert:
                        bodies.Add(((UnaryExpression)column.Expression.Body).Operand);
                        break;
                    default:
                        bodies.Add(column.Expression.Body);
                        break;
                }
            else if (column.Expressions != null)
            {
                //bodies.Add(column.Expressions.Body);
            }

        var newArrayExpression = Expression.NewArrayInit(typeof(object), bodies);

        query.ColumnPredicates = new[] { new ColumnPredicate<TObject>(Expression.Lambda<Func<TObject, IEnumerable<object>>>(newArrayExpression, expressionParameters)) };

        return query;
    }
}
