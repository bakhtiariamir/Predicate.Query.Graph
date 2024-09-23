using Dynamitey.DynamicObjects;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using Priqraph.Helper;
using Priqraph.Info.Database;
using System.Linq.Expressions;

namespace Priqraph.ExpressionHandler.Visitors;
public class DatabaseQueryableVisitor : Visitor<DatabaseQueryResult>
{
    protected ICacheInfoCollection CacheObjectCollection
    {
        get;
    }
    protected IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    public DatabaseQueryableVisitor(ParameterExpression parameterExpression, ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo) : base(parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
    }

    protected Expression VisitQuote(Expression expression)
    {
        var operand = ((UnaryExpression)expression).Operand;
        return operand;
    }

    protected override DatabaseQueryResult VisitConvert(UnaryExpression expression, string? memberName = null) => Visit(expression.Operand, memberName);

    protected bool IsNull(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Constant)
            return ((ConstantExpression)expression).Value == null;

        return false;
    }

    protected Type? GetValueType(object? value) => value?.GetType();
    protected IEnumerable<IColumnPropertyInfo>? GetProperty(Expression expression, IDatabaseObjectInfo objectInfo, ICacheInfoCollection cacheObjectCollection, bool isCondition = false)
    {
        ICollection<IColumnPropertyInfo>? properties = null;
        if (expression.NodeType == ExpressionType.NewArrayInit)
            properties = GetMemberColumnProperties(expression, objectInfo, cacheObjectCollection);
        else
        {
            var mainProperty = GetMemberColumnProperty(expression, objectInfo, cacheObjectCollection);
            if (mainProperty != null)
                properties = new List<IColumnPropertyInfo> { mainProperty };
        }

        if (properties is not { Count: > 0 })
            yield break;

        foreach (var property in properties)
            if (!isCondition)
            {
                if (property.DataType == ColumnDataType.Object &&  property.TryExpandProperty(cacheObjectCollection, out var childProperties) && childProperties is { Count: > 0 })
                {
                    foreach (var childProperty in childProperties)
                        yield return childProperty;
                }
                else
                    yield return property;
            }
            else
                yield return property;
    }

    private IColumnPropertyInfo? GetMemberColumnProperty(Expression expr, IDatabaseObjectInfo databaseObjectInfo, ICacheInfoCollection cacheInfoCollection)
    {
        IColumnPropertyInfo? property = new ColumnPropertyInfo();
        if (expr is MemberExpression memberExpression)
        {
            if (memberExpression.Expression == null)
                throw new NotFoundException(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

            var memberInfo = memberExpression.Member;
            if (!cacheInfoCollection.TryGetLastDatabaseObjectInfo(memberExpression.Expression.Type, out var parentObjectInfo))
            {
                switch (memberExpression.Expression.NodeType)
                {
                    case ExpressionType.Convert:
                        parentObjectInfo = ConvertObjectInfo(((UnaryExpression)memberExpression.Expression).Operand, cacheInfoCollection);
                        break;
                    case ExpressionType.Constant:
                        break;
                }
            }

            if (parentObjectInfo == null)
                throw new NotFoundException(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

            property = parentObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == memberInfo.Name)?.Clone() ?? throw new NotFoundException(memberExpression.Expression.Type.Name, memberInfo.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

            if (memberExpression.Expression is MemberExpression || memberExpression.Expression is ParameterExpression)
                property.SetRelationalObject(GetMemberColumnProperty(memberExpression.Expression, databaseObjectInfo, cacheInfoCollection)!);
        }
        else if (expr is ParameterExpression parameterExpression)
        {
            if (expr.Type == databaseObjectInfo.ObjectType)
            {
                property = new ColumnPropertyInfo(databaseObjectInfo.Schema, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, false, false, ColumnDataType.Object, DatabaseFieldType.Column, databaseObjectInfo.ObjectType);
            }
            else
            {
                var exprProperty = databaseObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == parameterExpression.Name);
                if (exprProperty != null)
                {
                    property = exprProperty;
                }
            }
        }
        else if (expr is NewArrayExpression arrayExpression)
            foreach (var item in arrayExpression.Expressions)
            {
                property = item.NodeType switch
                {
                    ExpressionType.Convert => GetMemberColumnProperty(((UnaryExpression)item).Operand, databaseObjectInfo, cacheInfoCollection),
                    ExpressionType.MemberAccess => GetMemberColumnProperty(item, databaseObjectInfo, cacheInfoCollection),
                    _ => property
                };
            }
        else
            throw new NotSupportedOperationException(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);

        if (property?.FieldType != DatabaseFieldType.Related)
            return property;

        return null;
    }
    private ICollection<IColumnPropertyInfo>? GetMemberColumnProperties(Expression expr, IDatabaseObjectInfo databaseObjectInfo, ICacheInfoCollection databaseCacheInfoCollection)
    {
        ICollection<IColumnPropertyInfo> properties = new List<IColumnPropertyInfo>();
        if (expr is NewArrayExpression arrayExpression)
        {
            foreach (var item in arrayExpression.Expressions)
            {
                if (item.NodeType == ExpressionType.Convert)
                {
                    var convertedProperty = GetMemberColumnProperty(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection);
                    if (convertedProperty != null)
                        properties.Add(convertedProperty);
                }
                else
                {
                    var property = GetMemberColumnProperty(item, databaseObjectInfo, databaseCacheInfoCollection);
                    if (property != null)
                        properties.Add(property);
                }
            }
        }
        else
        {
            throw new NotSupportedOperationException(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
        }

        return properties;
    }

    private IDatabaseObjectInfo? ConvertObjectInfo(Expression convertExpr, ICacheInfoCollection cacheInfoCollection)
    {
        IDatabaseObjectInfo? objectInfo = null;
        objectInfo = convertExpr switch
        {
            UnaryExpression unaryExpression => ConvertObjectInfo(unaryExpression.Operand, cacheInfoCollection),
            ParameterExpression parameterExpression when !cacheInfoCollection.TryGetLastDatabaseObjectInfo(parameterExpression.Type, out objectInfo) => throw new System.Exception(),
            _ => objectInfo
        };

        return objectInfo;
    }
}