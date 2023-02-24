using Dynamitey;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerFilteringVisitor : DatabaseVisitor<DatabaseWhereClauseQueryPart>
{
    private int _constantIndex = 0;

    public SqlServerFilteringVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseWhereClauseQueryPart VisitAndAlso(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.And);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitConvert(UnaryExpression expression, string? memberName = null)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            return Visit(expression.Operand, memberName);

        var memberInfo = expression.Operand.Type.GetInterfaces().SelectMany(item => item.GetMember("Id")).FirstOrDefault() ?? throw new System.Exception(); //todo
        return Visit(Expression.MakeMemberAccess(expression.Operand, memberInfo));
    }

    protected override DatabaseWhereClauseQueryPart VisitNot(UnaryExpression expression)
    {
        var left = Visit(expression.Operand);
        var right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(null, null, null));
        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ColumnPropertyInfo != null && (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having))
            clauseType = ClauseType.Having;
        if (left.Parameter.ColumnPropertyInfo != null && left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateWhereClause(left.Parameter, right.Parameter, ConditionOperatorType.Not, clauseType));
    }

    protected override DatabaseWhereClauseQueryPart VisitOrElse(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.Or);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitGreaterThan(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.GreaterThan);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitGreaterThanOrEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.GreaterThanEqual);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitLessThan(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.LessThan);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitLessThanOrEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.LessThanEqual);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitEndsWith(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.RightLike);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitStartsWith(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.LeftLike);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitContains(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.Contains);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitInclude(MethodCallExpression expression, bool condition)
    {
        var whereClause = VisitCall(expression, condition ? ConditionOperatorType.In : ConditionOperatorType.NotIn);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitCheckValue(MethodCallExpression expression, bool condition)
    {
        var whereClause = VisitCall(expression, condition ? ConditionOperatorType.IsNotNull : ConditionOperatorType.IsNull);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitNotEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.NotEqual);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitEqual(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.Equal);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitEqual(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.Equal);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(expression.ToString(), expression.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        var field = fields.FirstOrDefault() ?? throw new NotFound(expression.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override DatabaseWhereClauseQueryPart VisitMember(MemberExpression expression)
    {
        if (expression.Expression != null && expression.Expression.NodeType != ExpressionType.Parameter)
        {
            if (expression.Expression is ConstantExpression constantExpression)
                return Visit(expression.Expression, expression.Member.Name, expression);

            return Visit(expression.Expression, expression.Member.Name);
        }

        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        var field = fields.FirstOrDefault() ?? throw new NotFound(expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override DatabaseWhereClauseQueryPart VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
    {
        memberName ??= "param";
        var value = GetValue(expression, memberExpression);

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(value, GetValueType(value), memberName));
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

    private DatabaseWhereClauseQueryPart VisitCall(MethodCallExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        DatabaseWhereClauseQueryPart? left = null;
        DatabaseWhereClauseQueryPart? right = null;
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
                    memberExpression = ReduceParentalExpression(memberExpr as MemberExpression ?? throw new InvalidOperationException()) as MemberExpression;
                    constantExpression = ReduceParentalExpression(expression.Arguments[1], memberExpression) as ConstantExpression;
                }
                else
                {
                    memberExpression = memberExpr;
                }
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

        return VisitDatabaseWhereClause(operatorType, right, left);
    }

    private DatabaseWhereClauseQueryPart VisitBinary(BinaryExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        DatabaseWhereClauseQueryPart? left = null;
        DatabaseWhereClauseQueryPart? right = null;
        MemberExpression? memberExpression = null;
        Expression? rightExpression = null;
        if (expression.Left is MemberExpression { Expression: { } } memberExpr)
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
        else
        {

        }

        switch (operatorType)
        {
            case ConditionOperatorType.Equal:
                if (IsNull(expression.Right))
                {
                    right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(null, null, null));
                    operatorType = ConditionOperatorType.IsNull;
                }
                else
                {
                    if (rightExpression == null)
                        throw new System.Exception(); //todo

                    right = Visit(rightExpression, memberExpression.Member.Name);
                }
                break;
            case ConditionOperatorType.NotEqual:
                if (IsNull(expression.Right))
                {
                    right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(null, null, null));
                    operatorType = ConditionOperatorType.IsNotNull;
                }
                else
                {
                    if (rightExpression == null)
                        throw new System.Exception(); //todo

                    right = Visit(rightExpression, memberExpression.Member.Name);
                }
                break;
            case ConditionOperatorType.And:
                break;
            case ConditionOperatorType.Or:
                break;
            default:
                right = Visit(expression.Right);
                break;
        }

        return VisitDatabaseWhereClause(operatorType, right, left);
    }

    private static DatabaseWhereClauseQueryPart VisitDatabaseWhereClause(ConditionOperatorType operatorType, DatabaseWhereClauseQueryPart right, DatabaseWhereClauseQueryPart left)
    {
        if (right.Parameter.ColumnPropertyInfo == null && right.Parameter.ParameterName.ToLower() == left.Parameter.ColumnPropertyInfo.ColumnName.ToLower())
            right.Parameter.SetParameterColumnInfo(left.Parameter.ColumnPropertyInfo);

        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having)
            clauseType = ClauseType.Having;
        if (left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateWhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }

    private static Expression ReduceParentalExpression(Expression expression, MemberExpression? mainMember = null)
    {
        switch (expression)
        {
            case MemberExpression { Expression: null }:
                if (expression is MemberExpression memberExpression)
                    return memberExpression;
                else
                    throw new System.Exception();//todo
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
                }


                return expression;

            default:
                throw new System.Exception(); //todo
        }
    }
}
