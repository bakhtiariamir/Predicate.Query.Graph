using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Helper;

public static class DatabaseExpressionHelper
{
    //public static IEnumerable<IColumnPropertyInfo>? GetProperty(this Expression expression, IDatabaseObjectInfo objectInfo, IDatabaseCacheInfoCollection cacheObjectCollection, bool isJoinColumn = false)
    //{
    //    Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, bool, IColumnPropertyInfo>? getMemberExpression = null;

    //    Func<Expression, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, ICollection<IColumnPropertyInfo>> getMemberExpressions = null;

    //    getMemberExpression = (expr, databaseObjectInfo, databaseCacheInfoCollection, isMain) =>
    //    {
    //        IColumnPropertyInfo property = new ColumnPropertyInfo();
    //        if (expr is MemberExpression memberExpression)
    //        {
    //            if (memberExpression.Expression == null)
    //                throw new NotFound(memberExpression.Type.Name, memberExpression.Member.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

    //            var memberInfo = memberExpression.Member;
    //            if (!databaseCacheInfoCollection.TryGet(memberExpression.Expression.Type.Name, out var parentObjectInfo))
    //                throw new NotFound(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

    //            if (parentObjectInfo == null)
    //                throw new NotFound(memberExpression.Expression.Type.Name, ExceptionCode.CachedObjectInfo);

    //            property = parentObjectInfo.PropertyInfos.FirstOrDefault(item => item.Name == memberInfo.Name)?.Clone() ?? throw new NotFound(memberExpression.Expression.Type.Name, memberInfo.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);
    //            if (memberExpression.Expression is MemberExpression)
    //                property.SetRelationalObject(getMemberExpression?.Invoke(memberExpression.Expression, databaseObjectInfo, databaseCacheInfoCollection, false)!);
    //        }
    //        else if (expr is ParameterExpression parameterExpression)
    //        {
    //            if (expr.Type == databaseObjectInfo.ObjectType)
    //            {
    //                property = new ColumnPropertyInfo(databaseObjectInfo.Schema, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, databaseObjectInfo.DataSet, false, false, ColumnDataType.Object, DatabaseFieldType.Column);
    //            }
    //        }
    //        else if (expr is NewArrayExpression arrayExpression)
    //        {
    //            foreach (var item in arrayExpression.Expressions)
    //            {
    //                if (item.NodeType == ExpressionType.Convert)
    //                    property = getMemberExpression.Invoke(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection, true);

    //                if (item.NodeType == ExpressionType.MemberAccess)
    //                    property = getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true);
    //            }
    //        }
    //        else
    //        {
    //            throw new Exception.NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
    //        }

    //        return property;
    //    };

    //    getMemberExpressions = (expr, databaseObjectInfo, databaseCacheInfoCollection) =>
    //    {
    //        ICollection<IColumnPropertyInfo> properties = new List<IColumnPropertyInfo>();
    //        if (expr is NewArrayExpression arrayExpression)
    //        {
    //            foreach (var item in arrayExpression.Expressions)
    //            {
    //                if (item.NodeType == ExpressionType.Convert)
    //                    properties.Add(getMemberExpression.Invoke(((UnaryExpression)item).Operand, databaseObjectInfo, databaseCacheInfoCollection, true));

    //                if (item.NodeType == ExpressionType.MemberAccess)
    //                    properties.Add(getMemberExpression.Invoke(item, databaseObjectInfo, databaseCacheInfoCollection, true));
    //            }
    //        }
    //        else
    //        {
    //            throw new Exception.NotSupported(expr.Type.Name, expr.NodeType.ToString(), ExceptionCode.DatabaseQueryGeneratorGetProperty);
    //        }

    //        return properties;
    //    };

    //    ICollection<IColumnPropertyInfo>? properties = null;
    //    if (expression.NodeType == ExpressionType.NewArrayInit)
    //        properties = getMemberExpressions(expression, objectInfo, cacheObjectCollection);
    //    else
    //    {
    //        properties = new[]
    //        {
    //            getMemberExpression.Invoke(expression, objectInfo, cacheObjectCollection, true)
    //        };
    //    }

    //    foreach (var property in properties)
    //    {
    //        if (!isJoinColumn)
    //        {
    //            if (property.TryExpandProperty(cacheObjectCollection, out var childProperties))
    //            {
    //                foreach (var childProperty in childProperties)
    //                    yield return childProperty;
    //            }
    //            else
    //                yield return property;
    //        }
    //        else
    //            yield return property;
    //    }
    //}

    public static object? GetObject(this ConstantExpression expression) => expression.Value;

    public static bool TryExpandProperty(this IColumnPropertyInfo parent, IDatabaseCacheInfoCollection databaseCacheInfoCollection, out ICollection<IColumnPropertyInfo>? expandedProperties)
    {
        expandedProperties = null;
        if (!databaseCacheInfoCollection.TryGet(parent.Name, out var objectInfo))
        {
            if (!databaseCacheInfoCollection.TryGet(parent.Type.Name, out objectInfo))
                return false;
        }

        if (objectInfo == null)
            throw new NotFound("DatabaseObjectInfo", parent.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

        expandedProperties = new List<IColumnPropertyInfo>();
        foreach (var child in objectInfo?.PropertyInfos.GetProperties(parent, !(objectInfo.DataSet == parent.Name || objectInfo.DataSet == parent.DataSet))!)
            expandedProperties.Add(child);

        return expandedProperties.Count > 0;
    }

    public static LambdaExpression GenerateGetPropertyExpression(this IColumnPropertyInfo @join, IDatabaseObjectInfo propertyObjectInfo, string? indexer = null) => propertyObjectInfo.ObjectType.GenerateGetPropertyExpression($"{join.DataSet}{join.Name}{indexer}", join.Name);

    public static LambdaExpression GenerateJoinExpression(this IColumnPropertyInfo @join, Type propertyObjectType, JoinType joinType, string? indexer = null)
    {
        var parameter = GenerateParameterExpression(propertyObjectType, $"{join.Parent.DataSet}{join.DataSet}{indexer}", join.Parent.Name, out var member);
        var returnType = member.Type;
        return member.GenerateJoinExpression(propertyObjectType, returnType, parameter);
    }

    public static Expression<Func<TObject, TResult>> CastExpression<TObject, TResult>(this Expression propertyExpression) => (Expression<Func<TObject, TResult>>)propertyExpression;

    private static LambdaExpression GenerateGetPropertyExpression(this Type objectType, string parameterName, string memberName)
    {
        var parameter = Expression.Parameter(objectType, parameterName);
        var member = Expression.Property(parameter, memberName);
        return GenerateGetPropertyExpression(objectType, parameter, member);
    }

    private static LambdaExpression GenerateGetPropertyExpression(this Type objectType, ParameterExpression parameter, MemberExpression member)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectType, typeof(object));
        var expression = Expression.Lambda(funcMember, member, parameter);
        return expression;
    }

    private static ParameterExpression GenerateParameterExpression(Type objectType, string parameterName, string memberName, out MemberExpression member)
    {
        var parameter = Expression.Parameter(objectType, parameterName);
        member = Expression.Property(parameter, memberName);
        return parameter;
    }

    private static LambdaExpression GenerateJoinExpression(this Expression body, Type objectInfo, Type returnType, params ParameterExpression[] parameters)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectInfo, returnType);
        var expression = Expression.Lambda(funcMember, body, parameters);
        return expression;
    }
}
