using Priqraph.Contract;
using System.Linq.Expressions;

namespace Priqraph.ExpressionHandler;

public abstract class Visitor<TResult>(ParameterExpression? parameterExpression)
{
    protected ParameterExpression? ParameterExpression
    {
        get;
    } = parameterExpression;

    private readonly IDictionary<string, object> _options = new Dictionary<string, object>();

    public void AddOption(string key, object value) => _options.Add(key, value);

    public void RemoveOption(string key) => _options.Remove(key);

    public bool GetOption(string key, out object? value) => _options.TryGetValue(key, out value);

    public TResult Generate(Expression expression) => Visit(expression);

    protected virtual TResult Visit(Expression expression, string? memberName = null, MemberExpression? memberExpression = null)
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
                return VisitConstant((ConstantExpression)expression, memberName, memberExpression);

            case ExpressionType.Convert:
                return VisitConvert((UnaryExpression)expression, memberName);

            case ExpressionType.New:
                return VisitNew((NewExpression)expression);

            case ExpressionType.Call:
                return ((MethodCallExpression)expression).Method.Name switch
                {
                    "LeftContains" => VisitStartsWith((MethodCallExpression)expression),
                    "RightContains" => VisitEndsWith((MethodCallExpression)expression),
                    "Contains" => VisitContains((MethodCallExpression)expression),
                    "Like" => VisitContains((MethodCallExpression)expression),
                    "In" => VisitInclude((MethodCallExpression)expression, true),
                    "NotIn" => VisitInclude((MethodCallExpression)expression, false),
                    "Equals" => VisitEqual((MethodCallExpression)expression),
                    "IsNull" => VisitCheckValue((MethodCallExpression)expression, false),
                    "IsNotNull" => VisitCheckValue((MethodCallExpression)expression, true),
                    "Where" => VisitWhere((MethodCallExpression)expression),
                    "Select" => VisitSelect((MethodCallExpression)expression),
                    "OrderBy" => VisitOrderBy((MethodCallExpression)expression),
                    "OrderByDescending" => VisitOrderByDescending((MethodCallExpression)expression),
                    "Max" => VisitMax((MethodCallExpression)expression),
                    "Take" => VisitTake((MethodCallExpression)expression),
                    "Skip" => VisitSkip((MethodCallExpression)expression),
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

    protected virtual TResult VisitInclude(MethodCallExpression expression, bool condition) => throw new NotImplementedException();

    protected virtual TResult VisitCheckValue(MethodCallExpression expression, bool condition) => throw new NotImplementedException();

    protected virtual TResult VisitAndAlso(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitOrElse(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitNot(UnaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitEqual(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitEqual(MethodCallExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitNotEqual(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitGreaterThan(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitGreaterThanOrEqual(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitLessThan(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitLessThanOrEqual(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null) => throw new NotImplementedException();

    protected virtual TResult VisitConvert(UnaryExpression expression, string? memberName = null) => throw new NotImplementedException();

    protected virtual TResult VisitNew(NewExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitCall(MethodCallExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitContains(MethodCallExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitStartsWith(MethodCallExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitEndsWith(MethodCallExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitLambda(LambdaExpression expression) => Visit(expression.Body);

    protected virtual TResult VisitMember(MemberExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitArrayIndex(BinaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitArrayLength(UnaryExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitParameter(ParameterExpression expression) => throw new NotImplementedException();

    protected virtual TResult VisitNewArray(NewArrayExpression expression) => throw new NotImplementedException();


    protected virtual TResult VisitSelect(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitWhere(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitSkip(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitTake(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitOrderBy(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitOrderByDescending(MethodCallExpression expression) => throw new NotImplementedException();
    protected virtual TResult VisitMax(MethodCallExpression expression) => throw new NotImplementedException();
}
