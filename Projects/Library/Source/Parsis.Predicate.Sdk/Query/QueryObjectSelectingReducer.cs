using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectSelectingReducer<TObject, TQueryType> : QueryObjectPartReducer<TObject, TQueryType> where TObject : IQueryableObject
    where TQueryType : Enum
{
    public override QueryObject<TObject, TQueryType> Reduce(QueryObject<TObject, TQueryType> query, ReduceType type)
    {
        return base.Visit(query, type);
    }

    protected override QueryObject<TObject, TQueryType> Generate(QueryObject<TObject, TQueryType> query)
    {
        return query;
    }

    protected override QueryObject<TObject, TQueryType> Decrease(QueryObject<TObject, TQueryType> query)
    {
        return query;
    }

    protected override QueryObject<TObject, TQueryType> Merge(QueryObject<TObject, TQueryType> query)
    {
        if (query.Columns == null || query.Columns.Count == 1) return query;
        var columns = new List<QueryColumn<TObject>>();
        var expressionParameters = query.Columns.SelectMany(item => item.Expression.Parameters).FirstOrDefault() ?? Expression.Parameter(typeof(TObject));

        var bodies = new List<Expression>();
        foreach (var column in query.Columns)
        {
            if (column.Expression != null)
            {
                switch (column.Expression.Body.NodeType)
                {
                    case ExpressionType.Convert:
                        bodies.Add(((UnaryExpression)column.Expression.Body).Operand);
                        break;
                    default:
                        bodies.Add(column.Expression.Body);
                        break;
                }
            }
            else if (column.Expressions != null)
            {
                //bodies.Add(column.Expressions.Body);
            }
        }

        var newArrayExpression = System.Linq.Expressions.Expression.NewArrayInit(typeof(object), bodies);

        query.Columns = new[] { new QueryColumn<TObject>(Expression.Lambda<Func<TObject, IEnumerable<object>>>(newArrayExpression, expressionParameters)) };

        return query;
    }
}
