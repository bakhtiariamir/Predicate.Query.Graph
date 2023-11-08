using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Cache;
using Priqraph.Helper;
using Priqraph.Info;
using System.Linq.Expressions;

namespace Priqraph.ExpressionHandler.Visitors;

public abstract class CacheVisitor<TResult> : Visitor<TResult> where TResult : ICacheQueryPart
{
    protected CacheVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression parameterExpression) : base(parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
    }

    protected ICacheInfoCollection CacheObjectCollection
    {
        get;
    }

    protected IObjectInfo<IPropertyInfo> ObjectInfo
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
    protected IEnumerable<IPropertyInfo>? GetProperty(Expression expression, IObjectInfo<IPropertyInfo> objectInfo, ICacheInfoCollection cacheObjectCollection, bool isCondition = false)
    {
        Func<Expression, IObjectInfo<IPropertyInfo>, ICacheInfoCollection, IObjectInfo<IPropertyInfo>?>? getConvertedObjectInfo = null;

        getConvertedObjectInfo = (convertExpr, mainObjectInfo, fullCacheInfoCollection) =>
        {
            IObjectInfo<IPropertyInfo>? objectInfo = null;
            if (convertExpr is UnaryExpression unaryExpression)
            {
                objectInfo = getConvertedObjectInfo?.Invoke(unaryExpression.Operand, mainObjectInfo, fullCacheInfoCollection);
            }
            else if (convertExpr is ParameterExpression parameterExpression)
            {
                if (!fullCacheInfoCollection.TryGetLastObjectInfo(parameterExpression.Type, out objectInfo))
                    throw new System.Exception(); //todo
            }

            return objectInfo;
        };

        Func<Expression, IObjectInfo<IPropertyInfo>, ICacheInfoCollection, bool, IPropertyInfo>? getMemberExpression = null;

        Func<Expression, IObjectInfo<IPropertyInfo>, ICacheInfoCollection, ICollection<IPropertyInfo>>? getMemberExpressions = null;

        getMemberExpression = (expr, mainObjectInfo, fullCacheInfoCollection, isMain) =>
        {
            IPropertyInfo property = new PropertyInfo();
            if (expr is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == null)
                    throw new NotFound(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

                var memberInfo = memberExpression.Member;
                if (!fullCacheInfoCollection.TryGetLastObjectInfo(memberExpression.Expression.Type, out var parentObjectInfo))
                {
                    if (memberExpression.Expression.NodeType == ExpressionType.Convert)
                    {
                        parentObjectInfo = getConvertedObjectInfo.Invoke(((UnaryExpression)memberExpression.Expression).Operand, mainObjectInfo, fullCacheInfoCollection);
                    }
                    else if (memberExpression.Expression.NodeType == ExpressionType.Constant)
                    {
                    }
                }

                if (parentObjectInfo == null)
                    throw new NotFound(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

                property = parentObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == memberInfo.Name)?.ClonePropertyInfo() ?? throw new NotFound(memberExpression.Expression.Type.Name, memberInfo.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty); //Todo
                //چون فقط یک سطخ هست ئدذ نمیخواد
                //if (memberExpression.Expression is MemberExpression || memberExpression.Expression is ParameterExpression)
                //    property.SetRelationalObject(getMemberExpression?.Invoke(memberExpression.Expression, databaseObjectInfo, databaseCacheInfoCollection, false)!);
            }
            else if (expr is ParameterExpression parameterExpression)
            {
                if (expr.Type == mainObjectInfo.ObjectType)
                {
                    //property = new PropertySelector(objectInfo.Schema, objectInfo.DataSet, objectInfo.DataSet, objectInfo.ObjectType.Name, false, false, ColumnDataType.Object, DatabaseFieldType.Column, objectInfo.ObjectType);

                    property = new PropertyInfo(false, mainObjectInfo.ObjectType.Name, false, false, false, ColumnDataType.Object, mainObjectInfo.ObjectType, false);
                }
                else
                {
                    var exprProperty = mainObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == parameterExpression.Name);
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
                        property = getMemberExpression.Invoke(((UnaryExpression)item).Operand, mainObjectInfo, fullCacheInfoCollection, true);

                    if (item.NodeType == ExpressionType.MemberAccess)
                        property = getMemberExpression.Invoke(item, mainObjectInfo, fullCacheInfoCollection, true);
                }
            }
            else if (expr is UnaryExpression unaryExpression)
            {
            }
            else
            {
                throw new NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);//Todo
            }

            return property;
        };

        getMemberExpressions = (expr, mainObjectInfo, fullCacheInfoCollection) =>
        {
            ICollection<IPropertyInfo> properties = new List<IPropertyInfo>();
            if (expr is NewArrayExpression arrayExpression)
            {
                foreach (var item in arrayExpression.Expressions)
                {
                    if (item.NodeType == ExpressionType.Convert)
                        properties.Add(getMemberExpression.Invoke(((UnaryExpression)item).Operand, mainObjectInfo, fullCacheInfoCollection, true));
                    else
                        properties.Add(getMemberExpression.Invoke(item, mainObjectInfo, fullCacheInfoCollection, true));
                }
            }
            else
            {
                throw new NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty); //Todo
            }

            return properties;
        };

        ICollection<IPropertyInfo>? properties = null;
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
            yield return property;
        }
    }
}
