using Dynamitey;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;
using System.Reflection;
using Priqraph.Generator;
using Priqraph.Generator.Database;

namespace Priqraph.Sql.Generator.Visitors;
public class FilterVisitor : DatabaseVisitor<FilterQueryFragment>
{
    private int _constantIndex = 0;

    public FilterVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override FilterQueryFragment VisitAndAlso(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return CreateFilterClause(ConditionOperatorType.And, right, left);
    }

    protected override FilterQueryFragment VisitOrElse(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return CreateFilterClause(ConditionOperatorType.Or, right, left);
    }

    protected override FilterQueryFragment VisitConvert(UnaryExpression expression, string? memberName = null)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            return Visit(expression.Operand, memberName);

        var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
        return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    }

    protected override FilterQueryFragment VisitNot(UnaryExpression expression)
    {
        var left = Visit(expression.Operand);
        var right = FilterQueryFragment.Create(FilterProperty.CreateParameterClause(null, null, null));
        if (right.Parameter?.PartType == PartType.ParameterInfo && left.Parameter?.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter?.ColumnPropertyInfo != null && (left.Parameter.ClauseType == ClauseType.Having || right.Parameter?.ClauseType == ClauseType.Having))
            clauseType = ClauseType.Having;
        if (left.Parameter is { ColumnPropertyInfo: not null, ClauseType: ClauseType.Where or ClauseType.None } || right.Parameter?.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return FilterQueryFragment.Create(FilterProperty.CreateWhereClause(left.Parameter!, right.Parameter, ConditionOperatorType.Not, clauseType));
    }

    protected override FilterQueryFragment VisitGreaterThan(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.GreaterThan);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitGreaterThanOrEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.GreaterThanEqual);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitLessThan(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.LessThan);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitLessThanOrEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.LessThanEqual);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitEndsWith(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.RightLike);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitStartsWith(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.LeftLike);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitContains(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.Contains);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitInclude(MethodCallExpression expression, bool condition)
    {
        var whereClause = VisitCall(expression, condition ? ConditionOperatorType.In : ConditionOperatorType.NotIn);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitCheckValue(MethodCallExpression expression, bool condition)
    {
        var whereClause = VisitCall(expression, condition ? ConditionOperatorType.IsNotNull : ConditionOperatorType.IsNull);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitNotEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.NotEqual);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.Equal);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitEqual(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.Equal);
        return FilterQueryFragment.Create(whereClause.Parameter);
    }

    protected override FilterQueryFragment VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(expression.ToString(), expression.Name ?? "Expression.Name", ExceptionCode.DatabaseQueryFilteringGenerator);
        var field = fields.FirstOrDefault() ?? throw new NotFoundException(expression.Name ?? "Expression.Name", ExceptionCode.DatabaseQueryFilteringGenerator);

        return FilterQueryFragment.Create(new FilterProperty(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override FilterQueryFragment VisitMember(MemberExpression expression)
    {
        if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
        {
            if (expression.Expression is ConstantExpression constantExpression)
                return Visit(expression.Expression, expression.Member.Name, expression);

            return Visit(expression.Expression, expression.Member.Name);
        }

        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        var field = fields.FirstOrDefault() ?? throw new NotFoundException(expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return FilterQueryFragment.Create(new FilterProperty(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override FilterQueryFragment VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
    {
        memberName ??= "param";
        var value = GetValue(expression, memberExpression);

        return FilterQueryFragment.Create(FilterProperty.CreateParameterClause(value, GetValueType(value), memberName));
    }

    private static object? GetValue(ConstantExpression expression, MemberExpression? memberExpression)
    {
        if (memberExpression != null)
        {
            var container = expression.Value;
            var member = memberExpression.Member;
            return member switch
            {
                FieldInfo fieldInfo => fieldInfo.GetValue(container),
                PropertyInfo info => info.GetValue(container, null),
                _ => null
            };
        }

        return Expression.Lambda(expression).Compile().FastDynamicInvoke();
    }

    private FilterQueryFragment VisitCall(MethodCallExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        FilterQueryFragment? left;
        FilterQueryFragment? right;
        MemberExpression? memberExpression = null;
        ConstantExpression? constantExpression = null;

        if (expression.Object != null)
        {
            if (expression.Object is not MemberExpression objectExpression)
                throw new System.Exception(); //todo
            if (expression.Arguments[0] is not MemberExpression argMemberExpression)
                throw new System.Exception(); //todo

            memberExpression = ReduceParentalExpression(objectExpression) as MemberExpression ?? throw new System.Exception();
            constantExpression = ReduceParentalExpression(argMemberExpression, memberExpression) as ConstantExpression ?? throw new System.Exception();

            if (memberExpression == null || constantExpression == null)
                throw new NullReferenceException(); //todo

            left = Visit(memberExpression);
            right = Visit(constantExpression, memberExpression.Member.Name);
        }
        else
        {
            if (expression.Arguments[0] is MemberExpression memberExpr)
            {
                if (memberExpr.Expression is ConstantExpression)
                {
                    memberExpression = ReduceParentalExpression((expression.Arguments[1] as MemberExpression ?? throw new InvalidOperationException())) as MemberExpression;
                    constantExpression = ReduceParentalExpression(memberExpr, memberExpression) as ConstantExpression;
                }
                else if (memberExpr.Expression is UnaryExpression)
                {
                    memberExpression = ReduceParentalExpression(memberExpr) as MemberExpression;
                    constantExpression = ReduceParentalExpression(expression.Arguments[1], memberExpression) as ConstantExpression;
                }
                else
                    memberExpression = memberExpr;
            }
            else if (expression.Arguments[0] is ConstantExpression constantExpr)
            {
                constantExpression = constantExpr;
                memberExpression = expression.Arguments[1] as MemberExpression;
            }

            if (memberExpression == null || constantExpression == null)
                throw new NullReferenceException(); //todo

            left = Visit(memberExpression);
            right = Visit(constantExpression, memberExpression.Member.Name);
        }

        return CreateFilterClause(operatorType, right, left);
    }

    private FilterQueryFragment VisitBinary(BinaryExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        FilterQueryFragment? left = null;
        FilterQueryFragment? right;
        MemberExpression? memberExpression = null;
        Expression? rightExpression = null;
        if (expression.Left is MemberExpression { Expression: not null } memberExpr)
        {
            memberExpression = ReduceParentalExpression(memberExpr) as MemberExpression ?? throw new System.Exception();
            if (memberExpr.Expression is UnaryExpression)
            {
                if (expression.Right is not MemberExpression argMemberExpression)
                    throw new System.Exception(); //todo

                rightExpression = ReduceParentalExpression(argMemberExpression, memberExpression);
            }
            else
            {
                rightExpression = expression.Right;
            }

            left = Visit(memberExpression);
        }

        if (left is null)
            throw new ArgumentNullException("Expressn.Left");

        switch (operatorType)
        {
            case ConditionOperatorType.Equal:
                if (IsNull(expression.Right))
                {
                    right = FilterQueryFragment.Create(FilterProperty.CreateParameterClause(null, null, null));
                    operatorType = ConditionOperatorType.IsNull;
                }
                else
                {
                    if (rightExpression == null)
                        throw new System.Exception(); //todo

                    right = Visit(rightExpression, memberExpression!.Member.Name);
                }
                break;
            case ConditionOperatorType.NotEqual:
                if (IsNull(expression.Right))
                {
                    right = FilterQueryFragment.Create(FilterProperty.CreateParameterClause(null, null, null));
                    operatorType = ConditionOperatorType.IsNotNull;
                }
                else
                {
                    if (rightExpression == null)
                        throw new System.Exception(); //todo

                    right = Visit(rightExpression, memberExpression!.Member.Name);
                }
                break;
            default:
                right = Visit(expression.Right);
                break;
        }

        return CreateFilterClause(operatorType, right, left);
    }

    private static FilterQueryFragment CreateFilterClause(ConditionOperatorType operatorType, FilterQueryFragment right, FilterQueryFragment left)
    {
        if (left.Parameter is { PartType: PartType.ColumnInfo, ColumnPropertyInfo: null })
            throw new ArgumentNullException("Expression.Left.ColumnPropertyInfo");

        if (right.Parameter is { ParameterName: not null, ColumnPropertyInfo: null, PartType: PartType.ParameterInfo })
            right.Parameter.SetParameterColumnInfo(left.Parameter.ColumnPropertyInfo);

        if (right.Parameter is { PartType: PartType.ParameterInfo } && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ClauseType == ClauseType.Having || right.Parameter is { ClauseType: ClauseType.Having })
            clauseType = ClauseType.Having;

        if (left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter is { ClauseType: ClauseType.Where or ClauseType.None })
            clauseType = ClauseType.Where;

        return FilterQueryFragment.Create(FilterProperty.CreateWhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }

    private static Expression ReduceParentalExpression(Expression expression, MemberExpression? mainMember = null)
    {
        switch (expression)
        {
            //case MemberExpression { Expression: null }:
            //    if (expression is MemberExpression memberExpression)
            //        return memberExpression;

            //    throw new System.Exception();//todo
            case MemberExpression memberExpr:

                switch (memberExpr.Expression)
                {
                    case ConstantExpression constantExpression:
                        {
                            if (mainMember == null)
                                throw new System.Exception(); //todo
                            var value = GetValue(constantExpression, memberExpr);
                            return Expression.Constant(value);
                        }
                    case UnaryExpression unaryExpression:
                        {
                            var memberInfo = unaryExpression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember(memberExpr.Member.Name)).FirstOrDefault() ?? throw new System.Exception(); //todo

                            return Expression.MakeMemberAccess(unaryExpression.Operand, memberInfo);
                        }
                    case null:
                        {
                            if (expression is MemberExpression memberExpression)
                                return memberExpression;

                            throw new System.Exception();//todo
                        }
                }


                return expression;

            default:
                throw new System.Exception(); //todo
        }
    }
}
