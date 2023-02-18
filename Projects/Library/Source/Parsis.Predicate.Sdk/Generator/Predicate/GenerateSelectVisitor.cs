using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Predicate;
public class GenerateSelectVisitor<TObject> : PredicateGeneratorVisitor<TObject, Expression<Func<TObject, object>>> where TObject : IQueryableObject
{
    public override Expression<Func<TObject, object>> Generate(ExpressionType expressionType, object? value = null, string? methodName = null, params string[] properties)
    {
        var expression = Visit(expressionType, value, methodName, properties);
        return Expression.Lambda<Func<TObject, object>>(expression, Parameter);
    }


    protected override Expression VisitMember(object? value = null, params string[] properties)
    {
        Expression lastMember = Parameter;
        foreach (var property in properties)
        {
            var member = Expression.Property(lastMember, property);
            lastMember = member;
        }

        return lastMember;
    }
}
