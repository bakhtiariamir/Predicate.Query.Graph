using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Generator.Database;
using System.Linq.Expressions;
using Priqraph.Generator;

namespace Priqraph.Neo4j.Generator.Visitors;

public class SortVisitor : DatabaseVisitor<Neo4jSortQueryFragment>
{
    public SortVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override Neo4jSortQueryFragment VisitConvert(UnaryExpression expression, string? memberName = null)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            return Visit(expression.Operand, memberName);

        var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
        return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    }

    protected override Neo4jSortQueryFragment VisitMember(MemberExpression expression)
    {
        if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
        {
            return Visit(expression.Expression, expression.Member.Name);
        }

        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jSortQueryFragment.Create(fields.Select(item => new SortProperty(item)).ToArray());
    }

    protected override Neo4jSortQueryFragment VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jSortQueryFragment.Create(fields.Select(item => new SortProperty(item)).ToArray());
    }

    protected override Neo4jSortQueryFragment VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jSortQueryFragment.Create(fields.Select(item => new SortProperty(item)).ToArray());
    }
}
