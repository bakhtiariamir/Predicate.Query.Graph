using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using Priqraph.Helper;
using Priqraph.Info.Database;
using System.Linq.Expressions;

namespace Priqraph.ExpressionHandler.Visitors;

public abstract class DatabaseVisitor<TResult> : Visitor<TResult, IDatabaseObjectInfo, ICacheInfoCollection, IColumnPropertyInfo> where TResult : IDatabaseQueryPart
{
    protected DatabaseVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
        ParameterExpression = parameterExpression;
    }

    protected override ICacheInfoCollection CacheObjectCollection
    {
        get;
    }

    protected override IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    protected override ParameterExpression ParameterExpression
    {
        get;
    }

    protected override TResult VisitConvert(UnaryExpression expression, string? memberName = null) => Visit(expression.Operand, memberName);

    protected bool IsNull(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Constant)
            return (((ConstantExpression)expression).Value == null);

        return false;
    }

    protected Type? GetValueType(object? value) => value?.GetType();
    protected IEnumerable<IColumnPropertyInfo>? GetProperty(Expression expression, IDatabaseObjectInfo objectInfo, ICacheInfoCollection cacheObjectCollection, bool isCondition = false)
    {
        Func<Expression, IDatabaseObjectInfo, ICacheInfoCollection, IDatabaseObjectInfo?>? getConvertedObjectInfo = null;

        getConvertedObjectInfo = (convertExpr, mainObjectInfo, fullDatabaseCacheInfoCollection) =>
        {
            IDatabaseObjectInfo? objectInfo = null;
            if (convertExpr is UnaryExpression unaryExpression)
            {
                objectInfo = getConvertedObjectInfo?.Invoke(unaryExpression.Operand, mainObjectInfo, fullDatabaseCacheInfoCollection);
            }
            else if (convertExpr is ParameterExpression parameterExpression)
            {
                if (!fullDatabaseCacheInfoCollection.TryGetLastDatabaseObjectInfo(parameterExpression.Type, out objectInfo))
                    throw new System.Exception(); //todo
            }

            return objectInfo;
        };

        Func<Expression, IDatabaseObjectInfo, ICacheInfoCollection, bool, IColumnPropertyInfo?>? getMemberExpression = null;

        Func<Expression, IDatabaseObjectInfo, ICacheInfoCollection, ICollection<IColumnPropertyInfo?>>? getMemberExpressions = null;

        getMemberExpression = (expr, databaseObjectInfo, databaseCacheInfoCollection, isMain) =>
        {
            IColumnPropertyInfo property = new ColumnPropertyInfo();
            if (expr is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == null)
                    throw new NotFound(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

                var memberInfo = memberExpression.Member;
                if (!databaseCacheInfoCollection.TryGetLastDatabaseObjectInfo(memberExpression.Expression.Type, out var parentObjectInfo))
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

            if (property.FieldType != DatabaseFieldType.Related)
            {
                return property;
            }
            else
            {
                return null;
            }
        };

        getMemberExpressions = (expr, databaseObjectInfo, databaseCacheInfoCollection) =>
        {
            ICollection<IColumnPropertyInfo> properties = new List<IColumnPropertyInfo>();
            if (expr is NewArrayExpression arrayExpression)
            {
                foreach (var item in arrayExpression.Expressions)
                {
                    if (item.NodeType == ExpressionType.Convert)
                    {
                        var convertedProperty = getMemberExpression.Invoke(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection, true);
                        if (convertedProperty != null)
                            properties.Add(convertedProperty);
                    }
                    else
                    {
                        var property = getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true);
                        if (property != null)
                            properties.Add(property);
                    }
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
