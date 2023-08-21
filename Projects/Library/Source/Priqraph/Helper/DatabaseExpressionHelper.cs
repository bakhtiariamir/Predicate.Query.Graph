using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Query;
using System.Linq.Expressions;

namespace Priqraph.Helper;

public static class DatabaseExpressionHelper
{
    public static object? GetObject(this ConstantExpression expression) => expression.Value;

    public static bool TryExpandProperty(this IColumnPropertyInfo parent, ICacheInfoCollection databaseCacheInfoCollection, out ICollection<IColumnPropertyInfo>? expandedProperties)
    {
        expandedProperties = null;
        if (!databaseCacheInfoCollection.TryGetLastDatabaseObjectInfo(parent.Type, out var objectInfo))
            return false;


        if (objectInfo == null)
            throw new NotFound("DatabaseObjectInfo", parent.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

        var properties = objectInfo.PropertyInfos.Where(item => item.FieldType != DatabaseFieldType.Related).ToArray();
        expandedProperties = new List<IColumnPropertyInfo>();
        foreach (var child in properties.GetProperties(parent, !(parent.Parent == null && objectInfo.DataSet == parent.Name)))
        {
            // if (child.Type.IsAssignableTo(typeof(IQueryableObject)))
            // {
            //     if (!databaseCacheInfoCollection.TryGetLastDatabaseObjectInfo(child.Type, out var childObjectInfo))
            //     {
            //         var childKeyProperty = childObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException(child.Name, $"Column info is not complete for {child.Name}");
            //         childKeyProperty.Parent = child;
            //         expandedProperties.Add(childKeyProperty);
            //     }
            //     continue;
            // }
            //
            expandedProperties.Add(child);
        }

        return expandedProperties.Count > 0;
    }

    public static LambdaExpression GenerateGetPropertyExpression(this IColumnPropertyInfo @join, IDatabaseObjectInfo propertyObjectInfo, string? indexer = null) => propertyObjectInfo.ObjectType.GenerateGetPropertyExpression($"{join.DataSet}{join.Name}{indexer}", join.Name);

    public static LambdaExpression GenerateJoinExpression(this IColumnPropertyInfo @join, Type propertyObjectType, JoinType joinType, Type[] objectTypes, string? indexer = null)
    {
        var parameter = GenerateParameterExpression(GetParameterType(join), $"{GetParameterName(join)}{indexer}", MainMemberName(join.Parent), objectTypes, out var member);
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

    private static ParameterExpression GenerateParameterExpression(Type objectType, string parameterName, string memberName, Type[] objectTypeStructures, out MemberExpression member)
    {
        var parameter = Expression.Parameter(objectType, parameterName);
        Expression lastMember = parameter;
        var memberList = new List<Expression>();

        foreach (var property in memberName.Split('.'))
        {
            MemberExpression? propertyMember = null;
            if (parameter.Type.GetProperty(property) != null)
                propertyMember = Expression.Property(lastMember, property);
            else
            {
                Type? propertyType = null;
                foreach (var type in objectTypeStructures)
                    if (type.GetProperty(property) != null)
                        propertyType = type;

                if (propertyType == null)
                    throw new ArgumentNullException($"Member expression for {property} can not null.");

                propertyMember = Expression.Property(lastMember, propertyType, property);
            }

            lastMember = propertyMember ?? throw new ArgumentNullException($"Member expression for {property} can not null.");
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
