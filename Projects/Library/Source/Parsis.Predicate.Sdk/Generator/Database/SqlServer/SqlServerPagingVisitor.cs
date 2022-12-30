using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerPagingVisitor : DatabaseVisitor<DatabasePagingClauseQueryPart>
{
    public SqlServerPagingVisitor(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabasePagingClauseQueryPart VisitMember(MemberExpression expression)
    {
        return DatabasePagingClauseQueryPart.Create(new Page(10, 1));
    }

    protected override DatabasePagingClauseQueryPart VisitNew(NewExpression expression) => DatabasePagingClauseQueryPart.Create(Activator.CreateInstance<Page>(expression));
}


static class Activator
{
    public static T CreateInstance<T>(NewExpression newExpression)
    {
        var arguments = newExpression.Arguments.Select(expr => GetArgument((dynamic)expr)).ToArray();
        return (T)System.Activator.CreateInstance(typeof(T), arguments);
    }

    private static object GetArgument(ConstantExpression expression) => expression.Value;

    private static object GetArgument(UnaryExpression expression) => GetArgument((dynamic)expression.Operand);

    private static object GetArgument(Expression expression)
    {
        var convertExpr = Expression.Convert(expression, typeof(object));
        var lambda = Expression.Lambda<Func<object>>(convertExpr);
        return lambda.Compile().Invoke();
    }
}