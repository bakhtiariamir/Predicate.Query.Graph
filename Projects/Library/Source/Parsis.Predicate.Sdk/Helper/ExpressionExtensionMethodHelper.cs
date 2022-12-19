using System.Linq.Expressions;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Helper;
public static class ExpressionExtensionMethodHelper
{
    public static bool LeftContains(this string input, string phrase) => input.Trim().ToLower()[..Math.Min(input.Length, phrase.Length)].Equals(phrase.ToLower().Trim());

    public static bool RightContains(this string input, string phrase) => input.Trim().ToLower().Substring(Math.Max(input.Length - phrase.Length, 0), Math.Min(phrase.Length, input.Length)).Equals(phrase.ToLower().Trim());

    public static Expression<Func<HavingType, bool>> Having<TObject>(this TObject obj) => havingType => true;

}
