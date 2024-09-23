using Priqraph.Contract;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Query.Builders;
using System.Linq.Expressions;

namespace Priqraph.Sql.Generator.Visitors;
public class PageVisitor(
    ICacheInfoCollection cacheObjectCollection,
    IDatabaseObjectInfo objectInfo,
    ParameterExpression? parameterExpression)
    : DatabaseVisitor<PageQueryFragment>(cacheObjectCollection, objectInfo, parameterExpression)
{
    protected override PageQueryFragment VisitMember(MemberExpression expression) => PageQueryFragment.Create(new PageClause(0, 10));

    protected override PageQueryFragment VisitNew(NewExpression expression)
    {
        var instance = Extensions.Activator.CreateInstance<PageClause>(expression);
        return PageQueryFragment.Create(instance);
    }
}
