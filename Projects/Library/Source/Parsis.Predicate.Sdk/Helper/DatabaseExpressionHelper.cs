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
        foreach (var child in objectInfo?.PropertyInfos.GetProperties(parent, !(parent.Parent == null && objectInfo.DataSet == parent.Name))!)
            expandedProperties.Add(child);

        return expandedProperties.Count > 0;
    }

    public static LambdaExpression GenerateGetPropertyExpression(this IColumnPropertyInfo @join, IDatabaseObjectInfo propertyObjectInfo, string? indexer = null) => propertyObjectInfo.ObjectType.GenerateGetPropertyExpression($"{join.DataSet}{join.Name}{indexer}", join.Name);

    public static LambdaExpression GenerateJoinExpression(this IColumnPropertyInfo @join, Type propertyObjectType, JoinType joinType, string? indexer = null)
    {
        var parameter = GenerateParameterExpression(GetParameterType(join), $"{GetParameterName(join)}{indexer}", MainMemberName(join.Parent), out var member);
        var returnType = member.Type;
        return member.GenerateJoinExpression(GetParameterType(join), returnType, parameter);
    }

    private static string GetParameterName(IColumnPropertyInfo column) => column.Parent == null ? column.Name : GetParameterName(column.Parent!);

    private static Type GetParameterType(IColumnPropertyInfo column) => column.Parent == null ? column.Type : GetParameterType(column.Parent!);

    private static string MainMemberName(IColumnPropertyInfo column)
    {
        var memberName = string.Empty;
        if (column.Parent != null)
        {
            string? parentName = null;
            if (column.Parent.Parent != null)
                parentName = MainMemberName(column.Parent);

            memberName = parentName != null ? $"{parentName}.{column.Name}" : column.Name;
        }
        else memberName = column.Name;
        return memberName;
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
        Expression lastMember = parameter;
        var memberList = new List<Expression>();
        foreach (var property in memberName.Split('.'))
        {
            member = Expression.Property(lastMember, property);
            memberList.Add(member);
            lastMember = member;
        }
        member = (lastMember as MemberExpression)!;

        return parameter;
    }


    private static LambdaExpression GenerateJoinExpression(this Expression body, Type objectInfo, Type returnType, params ParameterExpression[] parameters)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectInfo, returnType);
        var expression = Expression.Lambda(funcMember, body, parameters);
        return expression;
    }
}
