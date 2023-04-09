using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Cache.MemoryCache;

public class PagingVisitor : CacheVisitor<CachePagingClauseQueryPart>
{
    public PagingVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override CachePagingClauseQueryPart VisitMember(MemberExpression expression) => CachePagingClauseQueryPart.Create(new Page(10, 1));

    protected override CachePagingClauseQueryPart VisitNew(NewExpression expression) => CachePagingClauseQueryPart.Create(ActivatorHelper.CreateInstance<Page>(expression));
}

