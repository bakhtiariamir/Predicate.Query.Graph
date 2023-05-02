using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectFiltering<TObject> : IQueryObjectPart<QueryObjectFiltering<TObject>, FilterPredicate<TObject>> where TObject : IQueryableObject
{
    private FilterPredicate<TObject> _filterPredicate;

    private QueryObjectFiltering(ReturnType returnType) => _filterPredicate = new FilterPredicate<TObject>(returnType);

    private QueryObjectFiltering(Expression<Func<TObject, bool>> expression) => _filterPredicate = new FilterPredicate<TObject>(expression);

    public static QueryObjectFiltering<TObject> Init(ReturnType returnType) => new(returnType);

    public static QueryObjectFiltering<TObject> Init(Expression<Func<TObject, bool>> expression) => new(expression);

    public static QueryObjectFiltering<TObject> Init(IEnumerable<Expression<Func<TObject, bool>>> expressions)
    {
        var expressionArray = expressions.ToArray();
        if (expressionArray.Length > 0)
        {
            var filterPredicate = Init(expressionArray[0]);
            for (var i = 1; i < expressionArray.Length; i++)
            {
                filterPredicate.And(expressionArray[i]);
            }

            return filterPredicate;
        }

        return new QueryObjectFiltering<TObject>(item => true);
    }

    public QueryObjectFiltering<TObject> And(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.And);

    public QueryObjectFiltering<TObject> Or(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.Or);

    public FilterPredicate<TObject> Return() => _filterPredicate;

    public Dictionary<string, string> GetQueryOptions() => new();

    private QueryObjectFiltering<TObject> CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _filterPredicate.Expression = connectorOperatorType switch {
            ConnectorOperatorType.And => _filterPredicate.Expression.AndAlsoExpression(expression),
            ConnectorOperatorType.Or => _filterPredicate.Expression.OrElseExpression(expression),
            _ => _filterPredicate.Expression
        };
        return this;
    }

    public QueryObjectFiltering<TObject> Validate() => this;
}

public class FilterPredicate<TObject> where TObject : IQueryableObject
{
    public Expression<Func<TObject, bool>>? Expression
    {
        get;
        set;
    }

    public ReturnType ReturnType
    {
        get;
        set;
    }

    internal FilterPredicate(Expression<Func<TObject, bool>> expression)
    {
        Expression = expression;
    }

    internal FilterPredicate(ReturnType returnType)
    {
        ReturnType = returnType;
    }

    internal FilterPredicate(Expression<Func<TObject, bool>> expression, ReturnType returnType)
    {
        Expression = expression;
        ReturnType = returnType;
    }

    public static FilterPredicate<TObject> CreateFilter(Expression<Func<TObject, bool>> expression) => new (expression);
}
