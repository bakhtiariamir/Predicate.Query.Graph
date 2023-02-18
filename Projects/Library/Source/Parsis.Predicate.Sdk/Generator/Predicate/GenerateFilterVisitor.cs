using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Generator.Predicate;
public class GenerateFilterVisitor<TObject> : PredicateGeneratorVisitor<TObject, Expression<Func<TObject, bool>>> where TObject : IQueryableObject
{
    private readonly IEnumerable<Type> _objectTypeStructures;

    public GenerateFilterVisitor(IEnumerable<Type> objectTypeStructures)
    {
        _objectTypeStructures = objectTypeStructures;
    }

    public override Expression<Func<TObject, bool>>  Generate(ExpressionType expressionType, object? value = null, string? methodName = null, params string[] properties)
    {
        var expression = Visit(expressionType, value, methodName, properties);
        return Expression.Lambda<Func<TObject, bool>>(expression, Parameter);
    }

    protected override Expression VisitMember(object? value = null, params string[] properties)
    {
        Expression lastMember = Parameter;

        foreach (var property in properties)
        {
            MemberExpression? member = null;
            if (Parameter.Type.GetProperty(property) != null)
                member = Expression.Property(lastMember, property);
            else
            {
                Type? propertyType = null;
                foreach (var type in _objectTypeStructures)
                    if (type.GetProperty(property) != null)
                        propertyType = type;

                if (propertyType == null)
                    throw new ArgumentNullException($"Member expression for {property} can not null.");

                member = Expression.Property(lastMember, propertyType, property);
            }

            lastMember = member ?? throw new ArgumentNullException($"Member expression for {property} can not null.");
        }

        return lastMember;
    }

    protected override Expression VisitEqual(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.Equal(member, valueExpression);
    }

    protected override Expression VisitNotEqual(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.NotEqual(member, valueExpression);
    }

    protected override Expression VisitGreaterThan(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.GreaterThan(member, valueExpression);
    }

    protected override Expression VisitGreaterThanOrEqual(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.GreaterThanOrEqual(member, valueExpression);
    }

    protected override Expression VisitLessThan(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.LessThan(member, valueExpression);
    }

    protected override Expression VisitLessThanOrEqual(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        return Expression.LessThanOrEqual(member, valueExpression);
    }

    protected override Expression VisitStartsWith(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        MethodInfo? methodInfo;
        if (value == null)
        {
            methodInfo = typeof(ExpressionExtensionMethodHelper).GetMethod("IsNull") ?? throw new NotImplementedException($"Method with these conditions MethodName:StartWith didn't implemented for this situation");
        }
        else
        {
            methodInfo = value.GetType().GetMethod("LeftContains") ?? throw new NotImplementedException($"Method with these conditions MethodName:StartWith didn't implemented for this situation");
        }

        return Expression.Call(valueExpression, methodInfo, member);
    }

    protected override Expression VisitEndsWith(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        MethodInfo? methodInfo;
        if (value == null)
        {
            methodInfo = typeof(ExpressionExtensionMethodHelper).GetMethod("IsNull") ?? throw new NotImplementedException($"Method with these conditions MethodName:EndWith didn't implemented for this situation");
        }
        else
        {
            methodInfo = value.GetType().GetMethod("RightContains") ?? throw new NotImplementedException($"Method with these conditions MethodName:EndWith didn't implemented for this situation");
        }

        return Expression.Call(valueExpression, methodInfo, member);
    }

    protected override Expression VisitContains(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        MethodInfo? methodInfo;
        if (value == null)
        {
            methodInfo = typeof(ExpressionExtensionMethodHelper).GetMethod("IsNull") ?? throw new NotImplementedException($"Method with these conditions MethodName:Contains didn't implemented for this situation");
        }
        else
        {
            if (value is string)
            {
                methodInfo = value.GetType().GetMethod("Contains") ?? throw new NotImplementedException($"Method with these conditions MethodName:Contains didn't implemented for this situation");
            }
            else
            {
                methodInfo = value.GetType().GetMethod("In") ?? throw new NotImplementedException($"Method with these conditions MethodName:Contains didn't implemented for this situation");
            }
            
        }

        return Expression.Call(valueExpression, methodInfo, member);
    }

    protected override Expression VisitInclude(bool condition, object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        var valueExpression = Visit(ExpressionType.Constant, value);
        MethodInfo? methodInfo;
        if (value == null)
        {
            methodInfo = (condition ? typeof(ExpressionExtensionMethodHelper).GetMethod("IsNull") : typeof(ExpressionExtensionMethodHelper).GetMethod("IsNotNull")) ?? throw new NotImplementedException($"Method with these conditions Check:{condition.ToString()} MethodName:Include didn't implemented for this situation");
        }
        else
        {
            Type elementType = value.GetType().GetElementType()!;
            methodInfo = (condition ? typeof(ExpressionExtensionMethodHelper).GetMethod("In", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!.MakeGenericMethod(elementType) : typeof(ExpressionExtensionMethodHelper).GetMethod("NotIn", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!.MakeGenericMethod(elementType)) ?? throw new NotImplementedException($"Method with these conditions Check:{condition} MethodName:Include didn't implemented for this situation");
        }

        return Expression.Call(null, methodInfo, valueExpression, member);
    }

    protected override Expression VisitNot(object? value = null, params string[] properties)
    {
        var member = Visit(ExpressionType.MemberAccess, value, properties: properties);
        return Expression.Not(member);
    }
}

    //public static Expression<Func<TObject, bool>> GetEquality(object value, params string[] properties)
    //{
    //    var parameter = Expression.Parameter(typeof(TObject), "source");

    //    Expression lastMember = parameter;

    //    for (var i = 0; i < properties.Length; i++)
    //    {
    //        var member = Expression.Property(lastMember, properties[i]);
    //        lastMember = member;
    //    }

    //    Expression valueExpression = Expression.Constant(value);
    //    Expression equalityExpression = Expression.Equal(lastMember, valueExpression);
    //    var lambda = Expression.Lambda<Func<TObject, bool>>(equalityExpression, parameter);
    //    return lambda;
    //}

//}
