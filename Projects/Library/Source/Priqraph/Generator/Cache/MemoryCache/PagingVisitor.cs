using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Query.Builders;
using System.Linq.Expressions;

namespace Priqraph.Generator.Cache.MemoryCache;

public class PagingVisitor : CacheVisitor<CachePagingClauseQueryPart>
{
    public PagingVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override CachePagingClauseQueryPart VisitMember(MemberExpression expression) => CachePagingClauseQueryPart.Create(new Page(10, 1));

    protected override CachePagingClauseQueryPart VisitNew(NewExpression expression) => CachePagingClauseQueryPart.Create(ActivatorHelper.CreateInstance<Page>(expression));
}

