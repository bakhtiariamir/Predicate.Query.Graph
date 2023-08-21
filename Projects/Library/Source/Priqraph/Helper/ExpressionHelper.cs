using Priqraph.ExpressionHandler;
using System.Linq.Expressions;

namespace Priqraph.Helper;

public static class ExpressionHelper
{
    public static bool IsNull(this Expression expression) => expression.NodeType == ExpressionType.Constant && (((ConstantExpression)expression).Value == null);

    public static bool IsNotNull(this Expression expression) => expression.NodeType == ExpressionType.Constant && (((ConstantExpression)expression).Value != null);

    public static Expression<Func<TObject, bool>> AndAlsoExpression<TObject>(this Expression<Func<TObject, bool>> firstExpression, Expression<Func<TObject, bool>> secondExpression)
    {
        var parameter = Expression.Parameter(typeof(TObject));

        var leftVisitor = new ReplaceExpressionVisitor(firstExpression.Parameters[0], parameter);
        var left = leftVisitor.Visit(firstExpression.Body);
        if (left == null)
            //ToDo : throw current type of exception
            throw new System.Exception("asdasd");

        var rightVisitor = new ReplaceExpressionVisitor(secondExpression.Parameters[0], parameter);
        var right = rightVisitor.Visit(secondExpression.Body);
        if (right == null)
            //ToDo : throw current type of exception
            throw new System.Exception("asdasd");

        return Expression.Lambda<Func<TObject, bool>>(Expression.AndAlso(left, right), parameter);
    }

    public static Expression<Func<TObject, bool>> OrElseExpression<TObject>(this Expression<Func<TObject, bool>> firstExpression, Expression<Func<TObject, bool>> secondExpression)
    {
        var parameter = Expression.Parameter(typeof(TObject));

        var leftVisitor = new ReplaceExpressionVisitor(firstExpression.Parameters[0], parameter);
        var left = leftVisitor.Visit(firstExpression.Body);
        if (left == null)
            //ToDo : throw current type of exception
            throw new System.Exception("asdasd");

        var rightVisitor = new ReplaceExpressionVisitor(secondExpression.Parameters[0], parameter);
        var right = rightVisitor.Visit(secondExpression.Body);
        if (right == null)
            //ToDo : throw current type of exception
            throw new System.Exception("asdasd");

        return Expression.Lambda<Func<TObject, bool>>(Expression.OrElse(left, right), parameter);
    }

    private static Expression? ReplaceParameterExpression<TObject>(this Expression<Func<TObject, bool>> expression, ParameterExpression typeName) => new ReplaceExpressionVisitor(expression.Parameters[0], typeName).Visit(expression.Body);
}
