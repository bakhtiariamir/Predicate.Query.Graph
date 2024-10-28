using Priqraph.Contract;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Query.Builders;
using System.Linq.Expressions;
using Priqraph.Exception;

namespace Priqraph.Neo4j.Generator.Visitors;
public class PageVisitor(
    ICacheInfoCollection cacheObjectCollection,
    IDatabaseObjectInfo objectInfo,
    ParameterExpression? parameterExpression)
    : DatabaseVisitor<Neo4jPageQueryFragment>(cacheObjectCollection, objectInfo, parameterExpression)
{
    protected override Neo4jPageQueryFragment VisitMember(MemberExpression expression) => Neo4jPageQueryFragment.Create(new PageClause(0, 10));

    protected override Neo4jPageQueryFragment VisitNew(NewExpression expression)
    {
        var instance = ActivatorHelper.CreateInstance<PageClause>(expression);
        return Neo4jPageQueryFragment.Create(instance);
    }
}
