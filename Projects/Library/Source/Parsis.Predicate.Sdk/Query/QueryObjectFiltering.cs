using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectFiltering<TObject> : IQueryObjectPart<QueryObjectFiltering<TObject>> where TObject : class
{
    private Expression<Func<TObject, bool>> _expression;

    public QueryObjectFiltering(Expression<Func<TObject, bool>> expression) => _expression = expression;

    public static QueryObjectFiltering<TObject> Init(Expression<Func<TObject, bool>> expression) => new(expression);

    public QueryObjectFiltering<TObject> And(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.And);

    public QueryObjectFiltering<TObject> Or(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.Or);

    public Expression<Func<TObject, bool>> Return() => _expression;

    private QueryObjectFiltering<TObject> CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _expression = connectorOperatorType switch
        {
            ConnectorOperatorType.And => _expression.AndAlsoExpression<TObject>(expression),
            ConnectorOperatorType.Or => _expression.OrElseExpression<TObject>(expression),
            _ => _expression
        };
        return this;
    }

    public QueryObjectFiltering<TObject> Validation() => this;
}

public class FilterPredicate<TObject> where TObject : class
{
    public Expression<Func<TObject, bool>> Expression
    {
        get;
    }

    public ConnectorOperatorType ConnectorOperator
    {
        get;
    }

    public FilterPredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorq)
    {
        Expression = expression;
        ConnectorOperator = connectorOperatorq;
    }
}
