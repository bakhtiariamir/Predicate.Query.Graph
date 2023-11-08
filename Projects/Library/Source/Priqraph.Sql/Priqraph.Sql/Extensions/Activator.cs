using System.Linq.Expressions;

namespace Priqraph.Sql.Extensions;
internal static class Activator
{
    public static T? CreateInstance<T>(NewExpression newExpression)
    {
        var arguments = newExpression.Arguments.Select(expr => ((dynamic)expr)).ToArray();
        return (T?)System.Activator.CreateInstance(typeof(T), arguments);
    }

    //private static object GetArgument(ConstantExpression expression) => expression.Value;

    //private static object GetArgument(UnaryExpression expression) => GetArgument((dynamic)expression.Operand);

    //private static object GetArgument(Expression expression)
    //{
    //    var convertExpr = Expression.Convert(expression, typeof(object));
    //    var lambda = Expression.Lambda<Func<object>>(convertExpr);
    //    return lambda.Compile().Invoke();
    //}
}
