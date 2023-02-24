using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerSortingVisitor : DatabaseVisitor<DatabaseOrdersByClauseQueryPart>
{
    public SqlServerSortingVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseOrdersByClauseQueryPart VisitConvert(UnaryExpression expression, string? memberName = null)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            return Visit(expression.Operand, memberName);

        var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
        return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    }

    protected override DatabaseOrdersByClauseQueryPart VisitMember(MemberExpression expression)
    {
        if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
        {
            return Visit(expression.Expression, expression.Member.Name);
        }

        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }

    protected override DatabaseOrdersByClauseQueryPart VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }

    protected override DatabaseOrdersByClauseQueryPart VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }
}
