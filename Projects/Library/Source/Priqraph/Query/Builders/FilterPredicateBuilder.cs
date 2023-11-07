using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class FilterPredicateBuilder<TObject> : IQueryObjectPart<FilterPredicateBuilder<TObject>, FilterPredicate<TObject>> where TObject : IQueryableObject
{
    private FilterPredicate<TObject> _filterPredicate;

    private FilterPredicateBuilder(ReturnType returnType)
    {
        _filterPredicate = new FilterPredicate<TObject>(returnType);
    }

    private FilterPredicateBuilder(Expression<Func<TObject, bool>> expression)
    {
        _filterPredicate = new FilterPredicate<TObject>(expression);
    }

    public static FilterPredicateBuilder<TObject> Init(ReturnType returnType) => new(returnType);

    public static FilterPredicateBuilder<TObject> Init(Expression<Func<TObject, bool>> expression) => new(expression);

    public static FilterPredicateBuilder<TObject> Init(IEnumerable<Expression<Func<TObject, bool>>> expressions)
    {
        var expressionArray = expressions.ToArray();
        if (expressionArray.Length > 0)
        {
            var filterPredicate = Init(expressionArray[0]);
            for (var i = 1; i < expressionArray.Length; i++)
                filterPredicate.And(expressionArray[i]);

            return filterPredicate;
        }

        return new FilterPredicateBuilder<TObject>(item => true);
    }

    public FilterPredicateBuilder<TObject> And(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.And);

    public FilterPredicateBuilder<TObject> Or(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.Or);

    public FilterPredicate<TObject> Return() => _filterPredicate;

    public Dictionary<string, string> GetQueryOptions() => new();

    private FilterPredicateBuilder<TObject> CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _filterPredicate.Expression = connectorOperatorType switch
        {
            ConnectorOperatorType.And => _filterPredicate.Expression.AndAlsoExpression(expression),
            ConnectorOperatorType.Or => _filterPredicate.Expression.OrElseExpression(expression),
            _ => _filterPredicate.Expression
        };
        return this;
    }

    public FilterPredicateBuilder<TObject> Validate() => this;
}