using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using Priqraph.Query.NewStructure.Handlers;
using System.Linq.Expressions;

namespace Priqraph.Query.NewStructure.Structure;
public class FilterPredicate<TObject> where TObject : IQueryableObject
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

    public void Add(Expression<Func<TObject, bool>> expression) => Expression ??= expression;
}

public class Where<TObject> : ReWriterHandler<TObject>, IWhereHandler<TObject> where TObject : IQueryableObject
{
    private readonly FilterPredicate<TObject> _predicate;

    public Where(Expression<Func<TObject, bool>> expression)
    {
        _predicate =  new FilterPredicate<TObject>(expression);
    }

    public static Where<TObject> Init(Expression<Func<TObject, bool>> expression) => new(expression);

    public static Where<TObject> Init(IEnumerable<Expression<Func<TObject, bool>>> expressions)
    {
        var expressionArray = expressions.ToArray();
        if (expressionArray.Length <= 0)
            return new Where<TObject>(item => true);

        var filterPredicate = Init(expressionArray[0]);
        for (var i = 1; i < expressionArray.Length; i++)
        {
            filterPredicate.And(expressionArray[i]);
        }

        return filterPredicate;

    }

    public Where<TObject> And(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.And);

    public Where<TObject> Or(Expression<Func<TObject, bool>> expression) => CreatePredicate(expression, ConnectorOperatorType.Or);

    public override void Handle(IQueryObject<TObject> queryObject) => throw new NotImplementedException();

    private Where<TObject> CreatePredicate(Expression<Func<TObject, bool>> expression, ConnectorOperatorType connectorOperatorType)
    {
        _predicate.Expression = connectorOperatorType switch
        {
            ConnectorOperatorType.And => _predicate.Expression.AndAlsoExpression(expression),
            ConnectorOperatorType.Or => _predicate.Expression.OrElseExpression(expression),
            _ => _predicate.Expression
        };
        return this;
    }

}