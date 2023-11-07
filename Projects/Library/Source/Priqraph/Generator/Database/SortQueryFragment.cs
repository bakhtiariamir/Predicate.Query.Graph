using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;

namespace Priqraph.Generator.Database;

public class SortQueryFragment : QueryFragment<ICollection<SortProperty>>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private SortQueryFragment(SortProperty sortProperty)
    {
        Parameter ??= new List<SortProperty>();
        Parameter.Add(sortProperty);
        SetText();
    }

    private SortQueryFragment(SortProperty[] columnSortPredicates)
    {
        Parameter = columnSortPredicates;
        SetText();
    }

    public static SortQueryFragment Create(params SortProperty[] columnSortPredicates) => new(columnSortPredicates);

    public static SortQueryFragment Create(SortProperty sortProperty) => new(sortProperty);

    public static SortQueryFragment Merged(IEnumerable<SortQueryFragment> orderByClauses) => new(orderByClauses.SelectMany(item => item.Parameter ?? new List<SortProperty>()).ToArray());

    private void SetText() => _text = Parameter is { Count: > 0 } ? $" ORDER BY {string.Join(", ", Parameter?.Select(columnSortPredicate => $"{SetColumnName(columnSortPredicate.ColumnPropertyInfo)} {SetDirection(columnSortPredicate.DirectionType)}") ?? new[] { "" })}" : string.Empty;

    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private string SetDirection(DirectionType directionType) => directionType switch {
        DirectionType.Asc => "asc",
        DirectionType.Desc => "desc",
        _ => throw new NotSupported("DirectionType", directionType.ToString(), ExceptionCode.DatabaseQuerySortingGenerator)
    };
}