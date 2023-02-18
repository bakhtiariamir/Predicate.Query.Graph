using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.ExpressionHandler;
public abstract class PredicateGeneratorVisitor<TObject, TResult> where TObject : IQueryableObject
{
    protected ParameterExpression Parameter
    {
        get;
    }

    protected PredicateGeneratorVisitor()
    {
        Parameter = Expression.Parameter(typeof(TObject), "source");
    }

    public abstract TResult Generate(ExpressionType expressionType, object? value = null, string? methodName = null, params string[] properties);


    public virtual Expression Visit(ExpressionType expressionType, object? value = null, string? methodName = null, params string[] properties)
    {
        switch (expressionType)
        {
            case ExpressionType.Not:
                return VisitNot(value, properties);

            case ExpressionType.Equal:
                return VisitEqual(value, properties);

            case ExpressionType.NotEqual:
                return VisitNotEqual(value, properties);

            case ExpressionType.GreaterThan:
                return VisitGreaterThan(value, properties);

            case ExpressionType.GreaterThanOrEqual:
                return VisitGreaterThanOrEqual(value, properties);

            case ExpressionType.LessThan:
                return VisitLessThan(value, properties);

            case ExpressionType.LessThanOrEqual:
                return VisitLessThanOrEqual(value, properties);

            case ExpressionType.Constant:
                return VisitConstant(value, properties);

            case ExpressionType.Convert:
                return VisitConvert(value, properties);

            case ExpressionType.New:
                return VisitNew(value, properties);

            case ExpressionType.Call:
                return methodName switch {
                    "LeftContains" => VisitStartsWith(value, properties),
                    "RightContains" => VisitEndsWith(value, properties),
                    "Contains" => VisitContains(value, properties),
                    "Like" => VisitContains(value, properties),
                    "In" => VisitInclude(true, value, properties),
                    "NotIn" => VisitInclude(false, value, properties),
                    _ => VisitCall(value, properties)
                };

            case ExpressionType.MemberAccess:
                return VisitMember(value, properties);

            case ExpressionType.ArrayIndex:
                return VisitArrayIndex(value, properties);

            case ExpressionType.ArrayLength:
                return VisitArrayLength(value, properties);

            case ExpressionType.Parameter:
                return VisitParameter(value, properties);
            case ExpressionType.NewArrayInit:
                return VisitNewArray(value, properties);
            default:
                throw new NotSupportedException($"NodeType: {expressionType}, is not supported.");
        }
    }

    protected virtual  Expression VisitInclude(bool condition, object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitAndAlso(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitOrElse(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitNot(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitEqual(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitNotEqual(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitGreaterThan(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitGreaterThanOrEqual(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitLessThan(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitLessThanOrEqual(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual Expression VisitConstant(object? value = null, params string[] properties) => Expression.Constant(value);

    protected virtual  Expression VisitConvert(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitNew(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitCall(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitContains(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitStartsWith(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitEndsWith(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitMember(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitArrayIndex(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitArrayLength(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitParameter(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }

    protected virtual  Expression VisitNewArray(object? value = null, params string[] properties)
    {
        throw new NotImplementedException();
    }
}
