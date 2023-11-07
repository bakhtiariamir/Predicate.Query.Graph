using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.ExpressionHandler;
using System.Linq.Expressions;

namespace Priqraph.Helper;

public static class ExpressionHelper
{
    public static object? ObjectValue(this ConstantExpression expression) => expression.Value;

    public static LambdaExpression GenerateJoinExpression(this IColumnPropertyInfo @join, Type propertyObjectType, JoinType joinType, Type[] objectTypes, string? indexer = null)
    {
        var parameter = GenerateParameterExpression(ParameterType(join), $"{ParameterName(join)}{indexer}", MainMemberName(join.Parent!), objectTypes, out var member);
        var returnType = member.Type;
        return member.GenerateJoinExpression(ParameterType(join), returnType, parameter);
    }

    public static Expression<Func<TObject, bool>> AndAlsoExpression<TObject>(this Expression<Func<TObject, bool>>? firstExpression, Expression<Func<TObject, bool>> secondExpression)
    {
        var parameter = Expression.Parameter(typeof(TObject));

        if (firstExpression != null)
        {
            var leftVisitor = new ReplaceExpressionVisitor(firstExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpression.Body);
            if (left == null)
                //ToDo : throw current type of exception
                throw new System.Exception("asdasd");

            var rightVisitor = new ReplaceExpressionVisitor(secondExpression.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpression.Body);
            if (right == null)
                //ToDo : throw current type of exception
                throw new System.Exception("asdasd");

            return Expression.Lambda<Func<TObject, bool>>(Expression.AndAlso(left, right), parameter);
        }

        return secondExpression;
    }

    public static Expression<Func<TObject, bool>> OrElseExpression<TObject>(this Expression<Func<TObject, bool>>? firstExpression, Expression<Func<TObject, bool>> secondExpression)
    {
        var parameter = Expression.Parameter(typeof(TObject));

        if (firstExpression != null)
        {
            var leftVisitor = new ReplaceExpressionVisitor(firstExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpression.Body);
            if (left == null)
                //ToDo : throw current type of exception
                throw new System.Exception("asdasd");

            var rightVisitor = new ReplaceExpressionVisitor(secondExpression.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpression.Body);
            if (right == null)
                //ToDo : throw current type of exception
                throw new System.Exception("asdasd");

            return Expression.Lambda<Func<TObject, bool>>(Expression.OrElse(left, right), parameter);
        }

        return secondExpression;
    }

    private static Expression? ReplaceParameterExpression<TObject>(this Expression<Func<TObject, bool>> expression, ParameterExpression typeName) => new ReplaceExpressionVisitor(expression.Parameters[0], typeName).Visit(expression.Body);

    private static string ParameterName(this IColumnPropertyInfo column) => column.Parent == null ? column.Name : ParameterName(column.Parent!);

    private static Type ParameterType(this IColumnPropertyInfo column) => column.Parent == null ? column.Type : ParameterType(column.Parent!);

    private static string MainMemberName(this IColumnPropertyInfo column)
    {
        string memberName;
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


    private static ParameterExpression GenerateParameterExpression(Type objectType, string parameterName, string memberName, Type[] objectTypeStructures, out MemberExpression member)
    {
        var parameter = Expression.Parameter(objectType, parameterName);
        Expression lastMember = parameter;

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

    private static LambdaExpression GeneratePropertyExpression(this Type objectType, string parameterName, string memberName)
    {
        var parameter = Expression.Parameter(objectType, parameterName);
        var member = Expression.Property(parameter, memberName);
        return GeneratePropertyExpression(objectType, parameter, member);
    }

    private static LambdaExpression GeneratePropertyExpression(this Type objectType, ParameterExpression parameter, MemberExpression member)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectType, typeof(object));
        var expression = Expression.Lambda(funcMember, member, parameter);
        return expression;
    }

    private static LambdaExpression GenerateJoinExpression(this Expression body, Type objectInfo, Type returnType, params ParameterExpression[] parameters)
    {
        var funcMember = typeof(Func<,>).MakeGenericType(objectInfo, returnType);
        var expression = Expression.Lambda(funcMember, body, parameters);
        return expression;
    }

    //public static LambdaExpression GenerateGetPropertyExpression(this IColumnPropertyInfo @join, IDatabaseObjectInfo propertyObjectInfo, string? indexer = null) => propertyObjectInfo.ObjectType.GenerateGetPropertyExpression($"{join.DataSet}{join.Name}{indexer}", join.Name);

    //public static Expression<Func<TObject, TResult>> CastExpression<TObject, TResult>(this Expression propertyExpression) => (Expression<Func<TObject, TResult>>)propertyExpression;

}
