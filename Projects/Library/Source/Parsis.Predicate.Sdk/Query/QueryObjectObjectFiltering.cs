using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectObjectFiltering<TObject> : IQueryObjectPart<QueryObjectObjectFiltering<TObject>> where TObject : class
{
    private readonly IList<FilterPredicate<TObject>> _filterPredicate;

    private QueryObjectObjectFiltering() => _filterPredicate = new List<FilterPredicate<TObject>>();

    public static QueryObjectObjectFiltering<TObject> Init() => new();

    public QueryObjectObjectFiltering<TObject> AndPredicate(Expression<Func<TObject, bool>> expression)
    {
        CreatePredicate(expression, ConnectorOperatorType.And);
        return this;
    }

    public QueryObjectObjectFiltering<TObject> OrPredicate(Expression<Func<TObject, bool>> expression)
    {

        CreatePredicate(expression, ConnectorOperatorType.Or);
        return this;
    }

    public IList<FilterPredicate<TObject>> Return() => _filterPredicate;

    private void CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _filterPredicate.Add(new FilterPredicate<TObject>(expression, connectorOperatorType));
    }

    public QueryObjectObjectFiltering<TObject> Validation() => this;
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
