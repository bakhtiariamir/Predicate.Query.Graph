using Priqraph.Contract;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Query.Builders;
using System.Linq.Expressions;

namespace Priqraph.Generator.Database.Visitors;

public class PageVisitor : DatabaseVisitor<PageQueryFragment>
{
    public PageVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override PageQueryFragment VisitMember(MemberExpression expression) => PageQueryFragment.Create(new Page(0, 10));

    protected override PageQueryFragment VisitNew(NewExpression expression)
    {
        var instance = ExpressionHandler.Activator.CreateInstance<Page>(expression);
        return PageQueryFragment.Create(instance);
    }
}
