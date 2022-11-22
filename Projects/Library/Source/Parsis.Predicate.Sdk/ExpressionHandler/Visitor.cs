using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.ExpressionHandler;
public abstract class Visitor<TResult, TObjectInfo, TCacheObjectCollection, TPropertyInfo> where TObjectInfo : IObjectInfo<TPropertyInfo> where TPropertyInfo : IPropertyInfo
{
    protected abstract TCacheObjectCollection CacheObjectCollection
    {
        get;
    }
    protected abstract TObjectInfo ObjectInfo
    {
        get;
    }
    protected abstract ParameterExpression ParameterExpression
    {
        get;
    }
    protected virtual TResult Visit(Expression expression, string? parameterName = null)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.AndAlso:
                return VisitAndAlso((BinaryExpression)expression);

            case ExpressionType.OrElse:
                return VisitOrElse((BinaryExpression)expression);

            case ExpressionType.Not:
                return VisitNot((UnaryExpression)expression);

            case ExpressionType.Equal:
                return VisitEqual((BinaryExpression)expression);

            case ExpressionType.NotEqual:
                return VisitNotEqual((BinaryExpression)expression);

            case ExpressionType.GreaterThan:
                return VisitGreaterThan((BinaryExpression)expression);

            case ExpressionType.GreaterThanOrEqual:
                return VisitGreaterThanOrEqual((BinaryExpression)expression);

            case ExpressionType.LessThan:
                return VisitLessThan((BinaryExpression)expression);

            case ExpressionType.LessThanOrEqual:
                return VisitLessThanOrEqual((BinaryExpression)expression);

            case ExpressionType.Constant:
                return VisitConstant((ConstantExpression)expression, parameterName);

            case ExpressionType.Convert:
                return VisitConvert((UnaryExpression)expression);

            case ExpressionType.New:
                return VisitNew((NewExpression)expression);

            case ExpressionType.Call:
                return ((MethodCallExpression)expression).Method.Name switch
                {
                    "LeftContains" => VisitStartsWith((MethodCallExpression)expression),
                    "RightContains" => VisitEndsWith((MethodCallExpression)expression),
                    "Contains" => VisitContain((MethodCallExpression)expression),
                    "Like" => VisitContain((MethodCallExpression)expression),
                    "In" => VisitInclude((MethodCallExpression)expression, true),
                    "NotIn" => VisitInclude((MethodCallExpression)expression, false),

                    _ => VisitCall((MethodCallExpression)expression)
                };
            case ExpressionType.Lambda:
                return VisitLambda((LambdaExpression)expression);

            case ExpressionType.MemberAccess:
                return VisitMember((MemberExpression)expression);

            case ExpressionType.ArrayIndex:
                return VisitArrayIndex((BinaryExpression)expression);

            case ExpressionType.ArrayLength:
                return VisitArrayLength((UnaryExpression)expression);

            case ExpressionType.Parameter:
                return VisitParameter((ParameterExpression)expression);
            case ExpressionType.NewArrayInit:
                return VisitNewArray((NewArrayExpression)expression);
            default:
                throw new NotSupportedException($"NodeType: {expression.NodeType}, is not supported.");
        }
    }

    protected virtual TResult VisitInclude(MethodCallExpression expression, bool condition)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitAndAlso(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitOrElse(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitNot(UnaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitEqual(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitNotEqual(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitGreaterThan(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitGreaterThanOrEqual(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitLessThan(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitLessThanOrEqual(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitConstant(ConstantExpression expression, string? parameterName = null)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitConvert(UnaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitNew(NewExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitCall(MethodCallExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitContain(MethodCallExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitStartsWith(MethodCallExpression expression)
    {
        throw new NotImplementedException();

    }

    protected virtual TResult VisitEndsWith(MethodCallExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitLambda(LambdaExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitMember(MemberExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitArrayIndex(BinaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitArrayLength(UnaryExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitParameter(ParameterExpression expression)
    {
        throw new NotImplementedException();
    }

    protected virtual TResult VisitNewArray(NewArrayExpression expression)
    {
        throw new NotImplementedException();
    }
}