using System.Net.Http.Headers;

namespace Priqraph.Helper;

public static class ExpressionExtensionMethodHelper
{
    public static bool LeftContains(this string input, string phrase) => input.Trim().ToLower()[..Math.Min(input.Length, phrase.Length)].Equals(phrase.ToLower().Trim());

    public static bool RightContains(this string input, string phrase) => input.Trim().ToLower().Substring(Math.Max(input.Length - phrase.Length, 0), Math.Min(phrase.Length, input.Length)).Equals(phrase.ToLower().Trim());

    public static bool Like(this string input, string phrase) => phrase.Contains(input);

    public static bool In<T>(this IEnumerable<T> items, T input) => items.Contains(input);

    public static bool NotIn<T>(this IEnumerable<T> items, T input) => !items.Contains(input);

    public static bool IsNull(this object? input) => input == null;

    public static bool IsNotNull(this object? input) => input != null;
    public static bool Between<T>(T item1, T item2, T input) => true;
}
