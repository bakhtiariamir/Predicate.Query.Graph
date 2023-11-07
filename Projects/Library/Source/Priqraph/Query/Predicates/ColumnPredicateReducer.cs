using Priqraph.Contract;
using Priqraph.DataType;
using System.Linq.Expressions;

namespace Priqraph.Query.Predicates;

public class ColumnPredicateReducer<TObject> : PredicateReducer<TObject> where TObject : IQueryableObject
{
    public override IQueryObject<TObject> Reduce(IQueryObject<TObject> query, ReduceType type)
    {
        return base.Visit(query, type);
    }

    protected override IQueryObject<TObject> Generate(IQueryObject<TObject> query)
    {
        return query;
    }

    protected override IQueryObject<TObject> Decrease(IQueryObject<TObject> query)
    {
        return query;
    }

    protected override IQueryObject<TObject> Merge(IQueryObject<TObject> query)
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
