using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Parsis.Predicate.Sdk.Generator.Database;
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

    protected override QueryPartType QueryPartType => QueryPartType.GroupBy;

    private DatabaseGroupByClauseQueryPart(GroupingPredicate parameter)
    {
        Parameter = parameter;
    }

    public static DatabaseGroupByClauseQueryPart Create(GroupingPredicate property) => new(property);

    public void GroupingText() => _text = $"{string.Join(", ", Parameter.GroupingColumns.Select(SetColumnSelector))}";

    public void HavingText() => _having = string.Join(" AND ", Parameter.GroupingHaving.Select(GetHavingText));

    private string GetHavingText(WhereClause clause)

    {
        if (clause.ColumnPropertyInfo == null) throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);
        return clause.Operator switch
        {
            ConditionOperatorType.NotEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} <> {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.GreaterThan => $"{GetHavingColumn(clause.ColumnPropertyInfo)} > {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.GreaterThanEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} >= {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.LessThan => $"{GetHavingColumn(clause.ColumnPropertyInfo)} < {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            ConditionOperatorType.LessThanEqual => $"{GetHavingColumn(clause.ColumnPropertyInfo)} <= {SetParameterName(clause.ColumnPropertyInfo, clause.Index)}",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new Exception.NotSupportedException(ExceptionCode.DatabaseQueryFilteringGenerator)
        };
    }

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"@P_{item.GetCombinedAlias()}_{index ?? 0}";

    private string GetHavingColumn(IColumnPropertyInfo property) => property.AggregationFunctionType switch
    {
        AggregationFunctionType.Count => $"COUNT(*)",
        AggregationFunctionType.Average => $"AVG({SetColumnSelector(property)})",
        AggregationFunctionType.Max => $"MAX({SetColumnSelector(property)})",
        AggregationFunctionType.Min => $"MIN({SetColumnSelector(property)})",
        AggregationFunctionType.Sum => $"SUM({SetColumnSelector(property)})",
        AggregationFunctionType.None or _ or null => $"{SetColumnSelector(property)}"
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