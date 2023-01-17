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
        ParameterExpression expressionParameters = query.Columns.SelectMany(item => item.Expression.Parameters).FirstOrDefault() ?? Expression.Parameter(typeof(TObject));
        //.Union(query.Columns.SelectMany(item => item.Expressions?.Parameters)?.DistinctBy(item => item.Type) ?? Enumerable.Empty<ParameterExpression>()).DistinctBy(item => item.Type);

        var bodies = new List<object>();
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
                //bodies.Add(column.Expression.Body);
            }
        }

        //  var ttt = Expression.Lambda<Func<TObject, IEnumerable<object>>>()  //(expressionParameters, bodies.ToArray());

        query.Columns = columns;
        return query;
    }
}
