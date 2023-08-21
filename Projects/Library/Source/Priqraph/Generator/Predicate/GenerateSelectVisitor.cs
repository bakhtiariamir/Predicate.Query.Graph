using Priqraph.Contract;
using Priqraph.ExpressionHandler;
using System.Linq.Expressions;

namespace Priqraph.Generator.Predicate;
public class GenerateSelectVisitor<TObject> : PredicateGeneratorVisitor<TObject, Expression<Func<TObject, object>>> where TObject : IQueryableObject
{
    private readonly IEnumerable<Type> _objectTypeStructures;

    public GenerateSelectVisitor(IEnumerable<Type> objectTypeStructures)
    {
        _objectTypeStructures = objectTypeStructures;
    }

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
}
