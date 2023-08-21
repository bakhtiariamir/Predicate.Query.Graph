using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;

namespace Priqraph.Generator.Database;

public class DatabaseGroupByClauseQueryPart : DatabaseQueryPart<GroupingPredicate>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    private string? _having;

    public string? Having
    {
        get => _having;
        set => _having = value;
    }

    private DatabaseGroupByClauseQueryPart(GroupingPredicate parameter)
    {
        Parameter = parameter;
    }

    public static DatabaseGroupByClauseQueryPart Create(GroupingPredicate property) => new(property);

    public void GroupingText() => _text = $"{string.Join(", ", Parameter.GroupingColumns.Select(SetColumnSelector))}";

    public void HavingText() => _having = Parameter.GroupingHaving is not null ? string.Join(" AND ", Parameter.GroupingHaving.Select(GetHavingText)) : "";

    private string GetHavingText(WhereClause clause)

    {
        if (clause.ColumnPropertyInfo == null) throw new NotFound(ExceptionCode.DatabaseQueryGroupByGenerator);
        return clause.Operator switch {
            ConditionOperatorType.NotEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} <> {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.GreaterThan => $"{GetHavingColumn(clause.ColumnPropertyInfo)} > {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.GreaterThanEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} >= {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.LessThan => $"{GetHavingColumn(clause.ColumnPropertyInfo)} < {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.LessThanEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} <= {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new NotSupported(ExceptionCode.DatabaseQueryFilteringGenerator)
        };
    }

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"@P_{item.GetCombinedAlias()}_{index ?? 0}";

    private string GetHavingColumn(IColumnPropertyInfo property) => property.AggregateFunctionType switch {
        AggregateFunctionType.Count => $"COUNT(*)",
        AggregateFunctionType.Average => $"AVG({SetColumnSelector(property)})",
        AggregateFunctionType.Max => $"MAX({SetColumnSelector(property)})",
        AggregateFunctionType.Min => $"MIN({SetColumnSelector(property)})",
        AggregateFunctionType.Sum => $"SUM({SetColumnSelector(property)})",
        AggregateFunctionType.None or _ or null => $"{SetColumnSelector(property)}"
    };

    private string SetColumnSelector(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";
}

public class GroupingPredicate
{
    public IEnumerable<IColumnPropertyInfo> GroupingColumns
    {
        get;
    }

    public IEnumerable<WhereClause>? GroupingHaving
    {
        get;
    }

    public GroupingPredicate(IEnumerable<IColumnPropertyInfo> groupingColumns, IEnumerable<WhereClause>? groupingHaving)
    {
        GroupingColumns = groupingColumns;
        GroupingHaving = groupingHaving;
    }
}
