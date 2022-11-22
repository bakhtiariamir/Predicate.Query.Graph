using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Info.Database;
using System.Linq.Expressions;
using Autofac.Core;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseExpressionHelper
{
    public static IEnumerable<IColumnPropertyInfo>? GetProperty(this Expression expression, IDatabaseObjectInfo objectInfo, IDatabaseCacheInfoCollection cacheObjectCollection)
    {
        Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, bool, IColumnPropertyInfo>? getMemberExpression = null;

        Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, ICollection<IColumnPropertyInfo>> getMemberExpressions  = null;

        getMemberExpression = (expr, databaseObjectInfo, databaseCacheInfoCollection, isMain) =>
        {
            IColumnPropertyInfo property = new ColumnPropertyInfo();
            if (expr is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == null)
                    throw new NotFoundException(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

                var memberInfo = memberExpression.Member;
                if (!databaseCacheInfoCollection.TryGet(memberExpression.Expression.Type.Name, out IDatabaseObjectInfo? parentObjectInfo))
                    throw new NotFoundException(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

                if (parentObjectInfo == null)
                    throw new NotFoundException(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

                property = parentObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == memberInfo.Name)?.Clone() ?? throw new NotFoundException(memberExpression.Expression.Type.Name, memberInfo.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);
                if (memberExpression.Expression is MemberExpression)
                    property.SetRelationalObject(getMemberExpression?.Invoke(memberExpression.Expression, databaseObjectInfo, databaseCacheInfoCollection, false)!);
            }
            else if (expr is ParameterExpression parameterExpression)
            {
                if (expr.Type == databaseObjectInfo.ObjectType)
                {
                    property = new ColumnPropertyInfo(databaseObjectInfo.Schema, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, false, ColumnDataType.Object, DatabaseFieldType.Column);
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
            else
            {
                throw new Exception.NotSupportedException(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
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

                    if (item.NodeType == ExpressionType.MemberAccess)
                        properties.Add(getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true));
                }
            }
            else
            {
                throw new Exception.NotSupportedException(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
            }

            return properties;
        };

        ICollection<IColumnPropertyInfo>? properties = null;
        if (expression.NodeType == ExpressionType.NewArrayInit)
            properties = getMemberExpressions(expression, objectInfo, cacheObjectCollection);
        else
        {
            properties = new[]
            {
                getMemberExpression.Invoke(expression, objectInfo, cacheObjectCollection, true)
            };
        }

        foreach (var property in properties)
        {
            if (property.TryExpandProperty(cacheObjectCollection, out ICollection<IColumnPropertyInfo>? childProperties))
            {
                foreach (var childProperty in childProperties)
                    yield return childProperty;
            }
            else
                yield return property;
        }
    }

    public static bool TryExpandProperty(this IColumnPropertyInfo parent, IDatabaseCacheInfoCollection databaseCacheInfoCollection, out ICollection<IColumnPropertyInfo>? expandedProperties)
    {
        expandedProperties = null;
        if (!databaseCacheInfoCollection.TryGet(parent.Name, out IDatabaseObjectInfo? objectInfo))
            return false;

        if (objectInfo == null)
            throw new NotFoundException("DatabaseObjectInfo", parent.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

        expandedProperties = new List<IColumnPropertyInfo>();
        foreach (var child in objectInfo?.PropertyInfos.GetProperties(parent, !(objectInfo.DataSet == parent.Name || objectInfo.DataSet == parent.DataSet))!)
            expandedProperties.Add(child);

        return expandedProperties.Count > 0;
    }

    //ToDo : split expression => create MemberExpression based on itemExpression 
    /// <summary>
    /// item => new object[] { Id, Username , Person.Id, Person.Name }
    /// item.Id , item.Username, item.Person.Id, item.Person.Name
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IEnumerable<Expression<Func<TObject, object>>> SplittingArrayExpression<TObject>(this Expression<Func<TObject, IEnumerable<object>>> expression)
    {

        if (expression.Body.NodeType == ExpressionType.NewArrayInit)
        {
            foreach (var itemExpression in ((NewArrayExpression)expression.Body).Expressions)
            {
                if (itemExpression.NodeType == ExpressionType.MemberAccess)
                {
                    yield return itemExpression.CastExpression<TObject, object>();
                }
                if (itemExpression.NodeType == ExpressionType.Convert)
                {
                    var operandExpression = ((MemberExpression)((UnaryExpression)itemExpression).Operand) ?? throw new NotFoundException(itemExpression.ToString(), ExceptionCode.DatabaseQueryGenerator);

                    Type objectType = operandExpression.Type ?? throw new NotFoundException(operandExpression.Member.Name, ExceptionCode.DatabaseQueryGenerator);
                    //var parameterExpression = Expression.Parameter(objectType, ((ParameterExpression)(MemberExpression)operandExpression.Expression).Name)
                    var parameterExpression = ((ParameterExpression)((MemberExpression)operandExpression.Expression!)?.Expression)?.Name;
                    //yield return objectType?.GenerateGetPropertyExpression("asda", "Asda");
                }
            }
        }
        yield break;
        ;
    }


    public static LambdaExpression GenerateGetPropertyExpression(this IColumnPropertyInfo @join, IDatabaseObjectInfo propertyObjectInfo, string? indexer = null) => propertyObjectInfo.ObjectType.GenerateGetPropertyExpression($"{@join.DataSet}{@join.Name}{indexer}", @join.Name);
    //{
    //    ParameterExpression parameter = Expression.Parameter(propertyObjectInfo.ObjectType, $"{@join.DataSet}{@join.Name}{indexer}");
    //    MemberExpression member = Expression.Property(parameter, @join.Name);
    //    var funcMember = typeof(Func<,>).MakeGenericType(propertyObjectInfo.ObjectType, typeof(object));
    //    var expression = Expression.Lambda(funcMember, member, parameter);
    //    return expression;
    //}

    private static LambdaExpression GenerateGetPropertyExpression(this Type objectType, string parameterName, string memberName)
    {
        ParameterExpression parameter = Expression.Parameter(objectType, parameterName);
        MemberExpression member = Expression.Property(parameter, memberName);
        return GenerateGetPropertyExpression(objectType, parameter, member);
    }

    private static LambdaExpression GenerateGetPropertyExpression(this Type objectType, ParameterExpression parameter, MemberExpression member)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectType, typeof(object));
        var expression = Expression.Lambda(funcMember, member, parameter);
        return expression;
    }
}   