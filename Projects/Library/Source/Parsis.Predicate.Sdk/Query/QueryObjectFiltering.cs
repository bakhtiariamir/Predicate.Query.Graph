using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectFiltering<TObject> : IQueryObjectPart<QueryObjectFiltering<TObject>, FilterPredicate<TObject>> where TObject : class
{
    private FilterPredicate<TObject> _filterPredicate;
    public QueryObjectFiltering(Expression<Func<TObject, bool>> expression) => _filterPredicate = new FilterPredicate<TObject>(expression);

    public static QueryObjectFiltering<TObject> Init(Expression<Func<TObject, bool>> expression) => new(expression);

    public QueryObjectFiltering<TObject> And(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.And);

    public QueryObjectFiltering<TObject> Or(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.Or);

    public FilterPredicate<TObject> Return() => _filterPredicate;

    private QueryObjectFiltering<TObject> CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _filterPredicate.Expression = connectorOperatorType switch
        {
            ConnectorOperatorType.And => _filterPredicate.Expression.AndAlsoExpression<TObject>(expression),
            ConnectorOperatorType.Or => _filterPredicate.Expression.OrElseExpression<TObject>(expression),
            _ => _filterPredicate.Expression
        };
        return this;
    }

    public QueryObjectFiltering<TObject> Validate() => this;
}

public class FilterPredicate<TObject> where TObject : class
{
    public Expression<Func<TObject, bool>> Expression
    {
        get;
        set;
    }

    public FilterPredicate(Expression<Func<TObject, bool>> expression)
    {
        Expression = expression;
    }
}
