using Dynamitey;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using Parsis.Predicate.Sdk.Info.Database;
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

    protected override DatabaseWhereClauseQueryPart VisitConvert(UnaryExpression expression)
    {
        return Visit(expression.Operand);
    }

    protected override DatabaseWhereClauseQueryPart VisitNot(UnaryExpression expression)
    {
        var operand = Visit(expression.Operand);

        return null;
        // SqlClause.CreateMerged($"(NOT {operand})", operand);
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
        if (expression.Expression is ConstantExpression constantExpression)
            return VisitConstant(constantExpression, expression);

        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        var field = fields.FirstOrDefault() ?? throw new NotFound(expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(field, clauseType: field.AggregateFunctionType != AggregateFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override DatabaseWhereClauseQueryPart VisitConstant(ConstantExpression expression, Expression? previousExpression = null)
    {
        object? value = null;
        var parameterName = "param";
        if (previousExpression is MemberExpression memberExpression)
        {
            parameterName = memberExpression.Member.Name;
            var container = expression.Value;
            var member = memberExpression.Member;
            if (member is FieldInfo fieldInfo)
            {
                value = fieldInfo.GetValue(container);
            }

            if (member is PropertyInfo info)
            {
                value = info.GetValue(container, null);
            }
        }

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(value, GetValueType(value), parameterName));
    }

    private DatabaseWhereClauseQueryPart VisitCall(MethodCallExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        DatabaseWhereClauseQueryPart? left = null;
        DatabaseWhereClauseQueryPart? right = null;
        if (operatorType == ConditionOperatorType.Contains)
        {
            if (expression.Arguments[0] is MemberExpression memberExpression)
            {
                if (memberExpression.Type.IsArray)
                {
                    operatorType = ConditionOperatorType.In;
                    left = Visit(expression.Arguments[1]);
                    var value = Expression.Lambda(memberExpression).Compile().FastDynamicInvoke();
                    right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(value, GetValueType(value), parameterName: memberExpression.Member.Name));
                }
                else
                {
                }
            }
        }
        else if (operatorType == ConditionOperatorType.Equal)
        {
            if (expression.Object is MemberExpression memberExpression)
            {
                left = Visit(memberExpression);
                var value = Expression.Lambda(expression.Arguments[0]).Compile().FastDynamicInvoke();
                right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(value, GetValueType(value), parameterName: memberExpression.Member.Name));
            }
        }

        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having)
            clauseType = ClauseType.Having;
        if (left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateWhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }

    private DatabaseWhereClauseQueryPart VisitBinary(BinaryExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        DatabaseWhereClauseQueryPart? left = null;
        DatabaseWhereClauseQueryPart? right = null;
        left = Visit(expression.Left);

        if (operatorType == ConditionOperatorType.Equal)
        {
            if (IsNull(expression.Right))
            {
                var property = new ColumnPropertyInfo();
                right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(null, null, null));
                operatorType = ConditionOperatorType.IsNull;
            }
            else
            {
                if (expression.Right is MemberExpression memberExpression)
                {
                    right = Visit(memberExpression.Expression, memberExpression);
                }
            }
        }
        else if (operatorType == ConditionOperatorType.NotEqual)
        {
            if (IsNull(expression.Right))
            {
                var property = new ColumnPropertyInfo();
                right = DatabaseWhereClauseQueryPart.Create(WhereClause.CreateParameterClause(null, null, null));
                operatorType = ConditionOperatorType.NotIsNull;
            }
            else
            {
                if (expression.Right is MemberExpression memberExpression)
                {
                    right = Visit(memberExpression.Expression);
                }
            }
        }
        else
        {
            right = Visit(expression.Right);
        }

        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ColumnPropertyInfo != null && (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having))
            clauseType = ClauseType.Having;
        if (left.Parameter.ColumnPropertyInfo != null && left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(WhereClause.CreateWhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }
}
