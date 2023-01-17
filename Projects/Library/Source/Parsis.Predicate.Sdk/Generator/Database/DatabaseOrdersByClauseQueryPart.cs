using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabaseOrdersByClauseQueryPart : DatabaseQueryPart<ICollection<ColumnSortPredicate>>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private DatabaseOrdersByClauseQueryPart(ColumnSortPredicate columnSortPredicate)
    {
        Parameter.Add(columnSortPredicate);
        SetText();
    }

    private DatabaseOrdersByClauseQueryPart(ColumnSortPredicate[] columnSortPredicates)
    {
        Parameter = columnSortPredicates;
        SetText();
    }

    public static DatabaseOrdersByClauseQueryPart Create(params ColumnSortPredicate[] columnSortPredicates) => new(columnSortPredicates);

    public static DatabaseOrdersByClauseQueryPart Create(ColumnSortPredicate columnSortPredicate) => new(columnSortPredicate);

    public static DatabaseOrdersByClauseQueryPart Merged(IEnumerable<DatabaseOrdersByClauseQueryPart> orderByClauses) => new DatabaseOrdersByClauseQueryPart(orderByClauses.SelectMany(item => item.Parameter).ToArray());

    private void SetText() => _text = $"ORDER BY {string.Join(", ", Parameter.Select(columnSortPredicate => $"{SetColumnName(columnSortPredicate.ColumnPropertyInfo)} {SetDirection(columnSortPredicate.DirectionType)}"))}";

    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";

    private string SetDirection(DirectionType directionType) => directionType switch {
        DirectionType.Asc => "asc",
        DirectionType.Desc => "desc",
        _ => throw new NotSupported("DirectionType", directionType.ToString(), ExceptionCode.DatabaseQuerySortingGenerator)
    };
}

public class ColumnSortPredicate
{
    public IColumnPropertyInfo ColumnPropertyInfo
    {
        get;
    }

    public DirectionType DirectionType
    {
        get;
    }

    public ColumnSortPredicate(IColumnPropertyInfo columnPropertyInfo, DirectionType directionType = DirectionType.Asc)
    {
        ColumnPropertyInfo = columnPropertyInfo;
        DirectionType = directionType;
    }
}
