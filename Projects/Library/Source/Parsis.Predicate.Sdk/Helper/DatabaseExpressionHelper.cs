using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Helper;

public static class DatabaseExpressionHelper
{
    public static object? GetObject(this ConstantExpression expression) => expression.Value;

    public static bool TryExpandProperty(this IColumnPropertyInfo parent, ICacheInfoCollection databaseCacheInfoCollection, out ICollection<IColumnPropertyInfo>? expandedProperties)
    {
        expandedProperties = null;
        if (!databaseCacheInfoCollection.TryGetLastDatabaseObjectInfo(parent.Type, out var objectInfo))
        {
            //if (!databaseCacheInfoCollection.TryGet(parent.Type.Name, out objectInfo))
            return false;
        }

        if (objectInfo == null)
            throw new NotFound("DatabaseObjectInfo", parent.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

        expandedProperties = new List<IColumnPropertyInfo>();
        foreach (var child in objectInfo?.PropertyInfos.GetProperties(parent, objectInfo.DataSet != parent.Name)!)
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
