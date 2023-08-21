using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Priqraph.Generator.Cache.MemoryCache;

public class SortingVisitor : CacheVisitor<CacheOrdersByClauseQueryPart>
{
    public SortingVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    //protected override CacheOrdersByClauseQueryPart VisitConvert(UnaryExpression expression, string? memberName = null)
    //{
    //    if (string.IsNullOrWhiteSpace(memberName))
    //        return Visit(expression.Operand, memberName);

    //    var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
    //    return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    //}

    //protected override CacheOrdersByClauseQueryPart VisitMember(MemberExpression expression)
    //{
    //    if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
    //    {
    //        return Visit(expression.Expression, expression.Member.Name);
    //    }

    //    var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator); //todo
    //    return CacheOrdersByClauseQueryPart.Create(fields.Select(item => new CacheSortPredicate(item)).ToArray());
    //}

    //protected override CacheOrdersByClauseQueryPart VisitParameter(ParameterExpression expression)
    //{
    //    var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator); //todo
    //    return CacheOrdersByClauseQueryPart.Create(fields.Select(item => new CacheSortPredicate(item)).ToArray());
    //}

    //protected override CacheOrdersByClauseQueryPart VisitNewArray(NewArrayExpression expression)
    //{
    //    var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator); //todo
    //    return CacheOrdersByClauseQueryPart.Create(fields.Select(item => new CacheSortPredicate(item)).ToArray());
    //}
}
