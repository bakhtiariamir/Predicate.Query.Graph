using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.ExpressionHandler;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Predicate;
public class GenerateSortVisitor<TObject> : PredicateGeneratorVisitor<TObject, SortPredicate<TObject>> where TObject : IQueryableObject
{
    public override SortPredicate<TObject> Generate(ExpressionType expressionType, object? value = null, string? methodName = null, params string[] properties)
    {
        switch (value)
        {
            case DirectionType directionType:
                {
                    var expression = Visit(expressionType, value, methodName, properties);
                    var lambda = Expression.Lambda<Func<TObject, object>>(expression, Parameter);
                    return new SortPredicate<TObject>(lambda, directionType);
                }
            default:
                throw new ArgumentException($"Direction of sort doesn't set correctly.");
        }
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
