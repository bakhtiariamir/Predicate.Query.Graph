using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Helper;
public static class QueryJoinHelper
{
    public static Expression<Func<TObject, TResult>> CastExpression<TObject, TResult>(this Expression propertyExpression) => (Expression<Func<TObject, TResult>>)propertyExpression;
}
