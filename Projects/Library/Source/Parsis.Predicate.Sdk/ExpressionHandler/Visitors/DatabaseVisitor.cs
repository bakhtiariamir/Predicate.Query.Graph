using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Database;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info.Database;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.ExpressionHandler.Visitors;

public abstract class DatabaseVisitor<TResult> : Visitor<TResult, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TResult : IDatabaseQueryPart
{
    protected DatabaseVisitor(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
        ParameterExpression = parameterExpression;
    }

    protected override IDatabaseCacheInfoCollection CacheObjectCollection
    {
        get;
    }

    protected override IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    protected override ParameterExpression? ParameterExpression
    {
        get;
    }

    protected override TResult VisitConvert(UnaryExpression expression) => Visit(expression.Operand);

    protected bool IsNull(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Constant)
            return (((ConstantExpression)expression).Value == null);

        return false;
    }

    protected Type? GetValueType(object? value)
    {
        if (value is null) return null;

        switch (value)
        {
            case int:
                return typeof(int);
            case long:
                return typeof(long);
            case float:
                return typeof(float);
            case double:
                return typeof(double);
            case decimal:
                return typeof(decimal);
            case byte:
                return typeof(byte);
            case bool:
                return typeof(bool);
            case char:
            case string:
                return typeof(string);
            default:
                return null;
        }
    }

    protected IEnumerable<IColumnPropertyInfo>? GetProperty(Expression expression, IDatabaseObjectInfo objectInfo, IDatabaseCacheInfoCollection cacheObjectCollection, bool isCondition = false)
    {
        Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IDatabaseObjectInfo?>? getConvertedObjectInfo = null;

        getConvertedObjectInfo = (convertExpr, mainObjectInfo, fullDatabaseCacheInfoCollection) =>
        {
            IDatabaseObjectInfo? objectInfo = null;
            if (convertExpr is UnaryExpression unaryExpression)
            {
                objectInfo = getConvertedObjectInfo?.Invoke(unaryExpression.Operand, mainObjectInfo, fullDatabaseCacheInfoCollection);
            }
            else if (convertExpr is ParameterExpression parameterExpression)
            {
                if (!fullDatabaseCacheInfoCollection.TryGet(parameterExpression.Type.Name, out objectInfo))
                    throw new System.Exception(); //todo
            }

            return objectInfo;
        };

        Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, bool, IColumnPropertyInfo>? getMemberExpression = null;

        Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, ICollection<IColumnPropertyInfo>> getMemberExpressions = null;

        getMemberExpression = (expr, databaseObjectInfo, databaseCacheInfoCollection, isMain) =>
        {
            IColumnPropertyInfo property = new ColumnPropertyInfo();
            if (expr is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == null)
                    throw new NotFound(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

                var memberInfo = memberExpression.Member;
                if (!databaseCacheInfoCollection.TryGet(memberExpression.Expression.Type.Name, out var parentObjectInfo))
                {
                    if (memberExpression.Expression.NodeType == ExpressionType.Convert)
                    {
                        parentObjectInfo = getConvertedObjectInfo.Invoke(((UnaryExpression)memberExpression.Expression).Operand, databaseObjectInfo, databaseCacheInfoCollection);
                    }
                    else if (memberExpression.Expression.NodeType == ExpressionType.Constant)
                    {
                    }
                }

                if (parentObjectInfo == null)
                    throw new NotFound(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

                property = parentObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == memberInfo.Name)?.Clone() ?? throw new NotFound(memberExpression.Expression.Type.Name, memberInfo.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);
                if (memberExpression.Expression is MemberExpression || memberExpression.Expression is ParameterExpression)
                    property.SetRelationalObject(getMemberExpression?.Invoke(memberExpression.Expression, databaseObjectInfo, databaseCacheInfoCollection, false)!);
            }
            else if (expr is ParameterExpression parameterExpression)
            {
                if (expr.Type == databaseObjectInfo.ObjectType)
                {
                    property = new ColumnPropertyInfo(databaseObjectInfo.Schema, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, false, false, ColumnDataType.Object, DatabaseFieldType.Column, databaseObjectInfo.ObjectType);
                }
            }
            else if (expr is NewArrayExpression arrayExpression)
            {
                foreach (var item in arrayExpression.Expressions)
                {
                    if (item.NodeType == ExpressionType.Convert)
                        property = getMemberExpression.Invoke(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection, true);

                    if (item.NodeType == ExpressionType.MemberAccess)
                        property = getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true);
                }
            }
            else if (expr is UnaryExpression unaryExpression)
            {
            }
            else
            {
                throw new NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
            }

            return property;
        };

        getMemberExpressions = (expr, databaseObjectInfo, databaseCacheInfoCollection) =>
        {
            ICollection<IColumnPropertyInfo> properties = new List<IColumnPropertyInfo>();
            if (expr is NewArrayExpression arrayExpression)
            {
                foreach (var item in arrayExpression.Expressions)
                {
                    if (item.NodeType == ExpressionType.Convert)
                        properties.Add(getMemberExpression.Invoke(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection, true));
                    else
                        properties.Add(getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true));
                }
            }
            else
            {
                throw new NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
            }

            return properties;
        };

        ICollection<IColumnPropertyInfo>? properties = null;
        if (expression.NodeType == ExpressionType.NewArrayInit)
            properties = getMemberExpressions(expression, objectInfo, cacheObjectCollection);
        else
        {
            properties = new[] {
                getMemberExpression.Invoke(expression, objectInfo, cacheObjectCollection, true)
            };
        }

        foreach (var property in properties)
        {
            if (!isCondition)
            {
                if (property.TryExpandProperty(cacheObjectCollection, out var childProperties))
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
    }
}
