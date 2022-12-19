using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info.Database;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerFilteringGenerator : SqlServerVisitor<DatabaseWhereClauseQueryPart>
{
    private int _index = 0;

    public SqlServerFilteringGenerator(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseWhereClauseQueryPart VisitAndAlso(BinaryExpression expression)
    {
        var whereClause = VisitBinary(expression, ConditionOperatorType.And);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
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

    protected override DatabaseWhereClauseQueryPart VisitContain(MethodCallExpression expression)
    {
        var whereClause = VisitCall(expression, ConditionOperatorType.Like);
        return DatabaseWhereClauseQueryPart.Create(whereClause.Parameter);
    }

    protected override DatabaseWhereClauseQueryPart VisitNotEqual(BinaryExpression expression)
    {
        return base.VisitNotEqual(expression);
    }

    protected override DatabaseWhereClauseQueryPart VisitEqual(BinaryExpression expression)
    {
        return base.VisitEqual(expression);
    }

    protected override DatabaseWhereClauseQueryPart VisitNot(UnaryExpression expression)
    {
        return base.VisitNot(expression);
    }

    protected override DatabaseWhereClauseQueryPart VisitInclude(MethodCallExpression expression, bool condition)
    {
        return base.VisitInclude(expression, condition);
    }

    protected override DatabaseWhereClauseQueryPart VisitMember(MemberExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        var field = fields.FirstOrDefault() ?? throw new NotFoundException(expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(field, clauseType: field.AggregationFunctionType != AggregationFunctionType.None ? ClauseType.Having : ClauseType.Where));
    }

    protected override DatabaseWhereClauseQueryPart VisitConstant(ConstantExpression expression)
    {
        var property = new ColumnPropertyInfo();
        var value = expression.Value ?? null;
        return DatabaseWhereClauseQueryPart.Create(new WhereClause(property, value, partType: PartType.ParameterInfo, clauseType: ClauseType.None));
    }

    private DatabaseWhereClauseQueryPart VisitCall(MethodCallExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        var left = Visit(expression.Arguments[0]);
        var right = Visit(expression.Arguments[1]);
        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having)
            clauseType = ClauseType.Having;
        if (left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }

    private DatabaseWhereClauseQueryPart VisitBinary(BinaryExpression expression, ConditionOperatorType operatorType = ConditionOperatorType.And)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        if (right.Parameter.PartType == PartType.ParameterInfo && left.Parameter.ColumnPropertyInfo != null && right.Parameter.ColumnPropertyInfo != null)
            right.Parameter.ColumnPropertyInfo.SetParameterData(left.Parameter.ColumnPropertyInfo.Schema, left.Parameter.ColumnPropertyInfo.DataSet, left.Parameter.ColumnPropertyInfo.Name, left.Parameter.ColumnPropertyInfo.ColumnName, left.Parameter.ColumnPropertyInfo.DataType);

        var clauseType = ClauseType.None;
        if (left.Parameter.ColumnPropertyInfo != null && (left.Parameter.ClauseType == ClauseType.Having || right.Parameter.ClauseType == ClauseType.Having))
            clauseType = ClauseType.Having;
        if (left.Parameter.ColumnPropertyInfo != null && left.Parameter.ClauseType is ClauseType.Where or ClauseType.None || right.Parameter.ClauseType is ClauseType.Where or ClauseType.None)
            clauseType = ClauseType.Where;

        return DatabaseWhereClauseQueryPart.Create(new WhereClause(left.Parameter, right.Parameter, operatorType, clauseType));
    }
}
