using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator;
using Priqraph.Generator.Database;

namespace Priqraph.Sql.Generator;
public class GroupByQueryFragment : DatabaseGroupByQueryFragment
{
    private GroupByQueryFragment(GroupByProperty parameter)
    {
        Parameter = parameter;
    }

    public static GroupByQueryFragment Create(GroupByProperty property) => new(property);

    public void GroupingText() => Text = Parameter is not null ? $"{string.Join(", ", Parameter.GroupingColumns.Select(SetColumnSelector))}" : "";

    public void HavingText() => Having = Parameter?.GroupingHaving is not null ? string.Join(" AND ", Parameter.GroupingHaving.Select(GetHavingText)) : "";

    private string GetHavingText(FilterProperty property)

    {
        if (property.ColumnPropertyInfo == null) throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);
        return property.Operator switch
        {
            ConditionOperatorType.NotEqual => $"{GetHavingColumn(property.ColumnPropertyInfo)} <> {SetParameterName(property.ColumnPropertyInfo, property.Index)}",
            ConditionOperatorType.GreaterThan => $"{GetHavingColumn(property.ColumnPropertyInfo)} > {SetParameterName(property.ColumnPropertyInfo, property.Index)}",
            ConditionOperatorType.GreaterThanEqual => $"{GetHavingColumn(property.ColumnPropertyInfo)} >= {SetParameterName(property.ColumnPropertyInfo, property.Index)}",
            ConditionOperatorType.LessThan => $"{GetHavingColumn(property.ColumnPropertyInfo)} < {SetParameterName(property.ColumnPropertyInfo, property.Index)}",
            ConditionOperatorType.LessThanEqual => $"{GetHavingColumn(property.ColumnPropertyInfo)} <= {SetParameterName(property.ColumnPropertyInfo, property.Index)}",
            //I think we should define parameter name in this item
            ConditionOperatorType.Set => $"",
            ConditionOperatorType.Between or _ => throw new NotSupportedOperationException(ExceptionCode.DatabaseQueryFilteringGenerator)
        };
    }

    private static string SetParameterName(IColumnPropertyInfo item, int? index) => $"@P_{item.GetCombinedAlias()}_{index ?? 0}";

    private string GetHavingColumn(IColumnPropertyInfo property) => property.AggregateFunctionType switch
    {
        AggregateFunctionType.Count => $"COUNT(*)",
        AggregateFunctionType.Average => $"AVG({SetColumnSelector(property)})",
        AggregateFunctionType.Max => $"MAX({SetColumnSelector(property)})",
        AggregateFunctionType.Min => $"MIN({SetColumnSelector(property)})",
        AggregateFunctionType.Sum => $"SUM({SetColumnSelector(property)})",
        AggregateFunctionType.None or _ or null => $"{SetColumnSelector(property)}"
    };

    private string SetColumnSelector(IColumnPropertyInfo item) => $"{item.GetSelector()}.[{item.ColumnName}]";
}