using Dynamitey;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Generator.Cache.MemoryCache;

public class FilteringVisitor : CacheVisitor<CacheWhereClauseQueryPart>
{
    public FilteringVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    //protected override CacheWhereClauseQueryPart VisitAndAlso(BinaryExpression expression)
    //{
    //    var left = Visit(expression.Left);
    //    var right = Visit(expression.Right);
    //    return VisitCacheCacheWhereClause(ConditionOperatorType.And, right, left);
    //}

    //protected override CacheWhereClauseQueryPart VisitConvert(UnaryExpression expression, string? memberName = null)
    //{
    //    if (string.IsNullOrWhiteSpace(memberName))
    //        return Visit(expression.Operand, memberName);

    //    var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
    //    return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    //}

    //protected override CacheWhereClauseQueryPart VisitNot(UnaryExpression expression)
    //{
    //    var left = Visit(expression.Operand);
    //    var right = CacheWhereClauseQueryPart.Create(CacheCacheWhereClause.CreateParameterClause(null, null, null));
    //    if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
    //        right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

    //    var clauseType = ClauseType.None;
    //    if (left.Parameter.ColumnPropertyInfo != null && (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having))
    //        clauseType = ClauseType.Having;
    //    if (left.Parameter.ColumnPropertyInfo != null && left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
    //        clauseType = ClauseType.Where;

    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.CreateCacheWhereClause(left.Parameter, right.Parameter, ConditionOperatorType.Not, clauseType));
    //}

    //protected override CacheWhereClauseQueryPart VisitOrElse(BinaryExpression expression)
    //{
    //    var left = Visit(expression.Left);
    //    var right = Visit(expression.Right);
    //    return VisitCacheCacheWhereClause(ConditionOperatorType.Or, right, left);
    //}

    //protected override CacheWhereClauseQueryPart VisitGreaterThan(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.GreaterThan);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitGreaterThanOrEqual(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.GreaterThanEqual);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitLessThan(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.LessThan);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitLessThanOrEqual(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.LessThanEqual);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitEndsWith(MethodCallExpression expression)
    //{
    //    var CacheWhereClause = VisitCall(expression, ConditionOperatorType.RightLike);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitStartsWith(MethodCallExpression expression)
    //{
    //    var CacheWhereClause = VisitCall(expression, ConditionOperatorType.LeftLike);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitContains(MethodCallExpression expression)
    //{
    //    var CacheWhereClause = VisitCall(expression, ConditionOperatorType.Contains);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitInclude(MethodCallExpression expression, bool condition)
    //{
    //    var CacheWhereClause = VisitCall(expression, condition ? ConditionOperatorType.In : ConditionOperatorType.NotIn);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitCheckValue(MethodCallExpression expression, bool condition)
    //{
    //    var CacheWhereClause = VisitCall(expression, condition ? ConditionOperatorType.IsNotNull : ConditionOperatorType.IsNull);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitNotEqual(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.NotEqual);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitEqual(BinaryExpression expression)
    //{
    //    var CacheWhereClause = VisitBinary(expression, ConditionOperatorType.Equal);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitEqual(MethodCallExpression expression)
    //{
    //    var CacheWhereClause = VisitCall(expression, ConditionOperatorType.Equal);
    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.Parameter);
    //}

    //protected override CacheWhereClauseQueryPart VisitParameter(ParameterExpression expression)
    //{
    //    var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(expression.ToString(), expression.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

    //    var field = fields.FirstOrDefault() ?? throw new NotFound(expression.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

    //    return CacheWhereClauseQueryPart.Create(new CacheWhereClause(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    //}

    //protected override CacheWhereClauseQueryPart VisitMember(MemberExpression expression)
    //{
    //    if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
    //    {
    //        if (expression.Expression is ConstantExpression constantExpression)
    //            return Visit(expression.Expression, expression.Member.Name, expression);

    //        return Visit(expression.Expression, expression.Member.Name);
    //    }

    //    var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

    //    var field = fields.FirstOrDefault() ?? throw new NotFound(expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

    //    return CacheWhereClauseQueryPart.Create(new CacheWhereClause(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    //}

    //protected override CacheWhereClauseQueryPart VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
    //{
    //    memberName ??= "param";
    //    var value = GetValue(expression, memberExpression);

    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.CreateParameterClause(value, GetValueType(value), memberName));
    //}

    //private static object? GetValue(ConstantExpression expression, MemberExpression? memberExpression)
    //{
    //    if (memberExpression != null)
    //    {
    //        var container = expression.Value;
    //        var member = memberExpression.Member;
    //        return member switch
    //        {
    //            FieldInfo fieldInfo => fieldInfo.GetValue(container),
    //            PropertySelector info => info.GetValue(container, null),
    //            _ => null
    //        };
    //    }

    //    return Expression.Lambda(expression).Compile().FastDynamicInvoke();
    //}

    //private CacheWhereClauseQueryPart VisitCall(MethodCallExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    //{
    //    CacheWhereClauseQueryPart? left = null;
    //    CacheWhereClauseQueryPart? right = null;
    //    MemberExpression? memberExpression = null;
    //    ConstantExpression? constantExpression = null;

    //    if (expression.Object != null)
    //    {
    //        if (expression.Object is not MemberExpression objectExpression)
    //            throw new System.Exception(); //todo
    //        if (expression.Arguments[0] is not MemberExpression argMemberExpression)
    //            throw new System.Exception(); //todo

    //        memberExpression = ReduceParentalExpression(objectExpression) as MemberExpression ?? throw new System.Exception();
    //        constantExpression = ReduceParentalExpression(argMemberExpression, memberExpression) as ConstantExpression ?? throw new System.Exception();

    //        if (memberExpression == null || constantExpression == null)
    //            throw new NullReferenceException(); //todo

    //        left = Visit(memberExpression);
    //        right = Visit(constantExpression, memberExpression.Member.Name);
    //    }
    //    else
    //    {
    //        if (expression.Arguments[0] is MemberExpression memberExpr)
    //        {
    //            if (memberExpr.Expression is ConstantExpression)
    //            {
    //                memberExpression = ReduceParentalExpression((expression.Arguments[1] as MemberExpression ?? throw new InvalidOperationException())) as MemberExpression;
    //                constantExpression = ReduceParentalExpression(memberExpr, memberExpression) as ConstantExpression;
    //            }
    //            else if (memberExpr.Expression is UnaryExpression)
    //            {
    //                memberExpression = ReduceParentalExpression(memberExpr as MemberExpression ?? throw new InvalidOperationException()) as MemberExpression;
    //                constantExpression = ReduceParentalExpression(expression.Arguments[1], memberExpression) as ConstantExpression;
    //            }
    //            else
    //            {
    //                memberExpression = memberExpr;
    //            }
    //        }
    //        else if (expression.Arguments[0] is ConstantExpression constantExpr)
    //        {
    //            constantExpression = constantExpr;
    //            memberExpression = expression.Arguments[1] as MemberExpression;
    //        }


    //        if (memberExpression == null || constantExpression == null)
    //            throw new NullReferenceException(); //todo

    //        left = Visit(memberExpression);
    //        right = Visit(constantExpression, memberExpression.Member.Name);
    //    }

    //    return VisitCacheCacheWhereClause(operatorType, right, left);
    //}

    //private CacheWhereClauseQueryPart VisitBinary(BinaryExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    //{
    //    CacheWhereClauseQueryPart? left = null;
    //    CacheWhereClauseQueryPart? right = null;
    //    MemberExpression? memberExpression = null;
    //    Expression? rightExpression = null;
    //    if (expression.Left is MemberExpression { Expression: { } } memberExpr)
    //    {
    //        memberExpression = ReduceParentalExpression(memberExpr) as MemberExpression ?? throw new System.Exception();
    //        if (memberExpr.Expression is UnaryExpression)
    //        {
    //            if (expression.Right is not MemberExpression argMemberExpression)
    //                throw new System.Exception(); //todo

    //            rightExpression = ReduceParentalExpression(argMemberExpression, memberExpression);
    //        }
    //        else
    //        {
    //            rightExpression = expression.Right;
    //        }

    //        left = Visit(memberExpression);
    //    }
    //    else
    //    {

    //    }

    //    switch (operatorType)
    //    {
    //        case ConditionOperatorType.Equal:
    //            if (IsNull(expression.Right))
    //            {
    //                right = CacheWhereClauseQueryPart.Create(CacheWhereClause.CreateParameterClause(null, null, null));
    //                operatorType = ConditionOperatorType.IsNull;
    //            }
    //            else
    //            {
    //                if (rightExpression == null)
    //                    throw new System.Exception(); //todo

    //                right = Visit(rightExpression, memberExpression.Member.Name);
    //            }
    //            break;
    //        case ConditionOperatorType.NotEqual:
    //            if (IsNull(expression.Right))
    //            {
    //                right = CacheWhereClauseQueryPart.Create(CacheWhereClause.CreateParameterClause(null, null, null));
    //                operatorType = ConditionOperatorType.IsNotNull;
    //            }
    //            else
    //            {
    //                if (rightExpression == null)
    //                    throw new System.Exception(); //todo

    //                right = Visit(rightExpression, memberExpression.Member.Name);
    //            }
    //            break;
    //        case ConditionOperatorType.And:
    //            break;
    //        case ConditionOperatorType.Or:
    //            break;
    //        default:
    //            right = Visit(expression.Right);
    //            break;
    //    }

    //    return VisitCacheCacheWhereClause(operatorType, right, left);
    //}

    //private static CacheWhereClauseQueryPart VisitCacheCacheWhereClause(ConditionOperatorType operatorType, CacheWhereClauseQueryPart right, CacheWhereClauseQueryPart left)
    //{
    //    if (right.Parameter.ParameterName != null && right.Parameter.ColumnPropertyInfo == null && right.Parameter.PartType == PartType.ParameterInfo && right.Parameter.ParameterName.ToLower() == left.Parameter.ColumnPropertyInfo.ColumnName.ToLower())
    //        right.Parameter.SetParameterColumnInfo(left.Parameter.ColumnPropertyInfo);

    //    if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
    //        right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

    //    var clauseType = ClauseType.None;
    //    if (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having)
    //        clauseType = ClauseType.Having;
    //    if (left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
    //        clauseType = ClauseType.Where;

    //    return CacheWhereClauseQueryPart.Create(CacheWhereClause.CreateCacheWhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    //}

    //private static Expression ReduceParentalExpression(Expression expression, MemberExpression? mainMember = null)
    //{
    //    switch (expression)
    //    {
    //        case MemberExpression { Expression: null }:
    //            if (expression is MemberExpression memberExpression)
    //                return memberExpression;
    //            else
    //                throw new System.Exception();//todo
    //        case MemberExpression memberExpr:

    //            switch (memberExpr.Expression)
    //            {
    //                case ConstantExpression constantExpression:
    //                    {
    //                        if (mainMember == null)
    //                            throw new System.Exception(); //todo
    //                        var value = GetValue(constantExpression, memberExpr);
    //                        return Expression.Constant(value);
    //                    }
    //                case UnaryExpression unaryExpression:
    //                    {
    //                        var memberInfo = unaryExpression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember(memberExpr.Member.Name)).FirstOrDefault() ?? throw new System.Exception(); //todo

    //                        return Expression.MakeMemberAccess(unaryExpression.Operand, memberInfo);
    //                    }
    //            }


    //            return expression;

    //        default:
    //            throw new System.Exception(); //todo
    //    }
    //}
}
