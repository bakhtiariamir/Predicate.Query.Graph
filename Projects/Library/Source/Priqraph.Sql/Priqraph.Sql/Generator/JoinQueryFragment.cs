using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;

namespace Priqraph.Sql.Generator;
public class JoinQueryFragment : DatabaseJoinQueryFragment
{
    private JoinQueryFragment(ICollection<JoinProperty> joinPredicates)
    {
        Parameter = joinPredicates;
    }

    private void SetText() => Text = string.Join(" ", Parameter?.Select(SetJoinText) ?? new[] { "" });

    private string SetJoinText(JoinProperty joinProperty)
    {
        var joinString = joinProperty.JoinType switch
        {
            JoinType.Inner => "INNER JOIN",
            JoinType.Left => "LEFT JOIN",
            JoinType.Right => "RIGHT JOIN",
            JoinType.Outer => "OUTER JOIN",
            _ => throw new NotSupported(joinProperty.JoinType.ToString(), ExceptionCode.DatabaseQueryJoiningGenerator)
        };
        //ToDo : Need PersonObjectInfo For Get id
        var joinItem = joinProperty.JoinObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException("JoinProperty");
        return $"{joinString} {joinProperty.JoinObjectInfo} AS [{joinProperty.JoinColumn}] ON [{joinProperty.JoinColumn}].[{joinItem.ColumnName}] = {joinProperty.JoinColumn.GetSelector()}.[{joinProperty.JoinColumn.ColumnName}]";
    }

    public static JoinQueryFragment Create(params JoinProperty[] joinPredicates)
    {
        var joinFragment = new JoinQueryFragment(joinPredicates);
        joinFragment.SetText();
        return joinFragment;
    }

    public static JoinQueryFragment Merged(IEnumerable<JoinQueryFragment> sqlQueries)
    {
        var joinFragment = new JoinQueryFragment(sqlQueries.SelectMany(properties => properties.Parameter ?? new List<JoinProperty>()).DistinctBy(item => new
        {
            item.JoinColumn,
            item.JoinType
        }).ToList());
        joinFragment.SetText();

        return joinFragment;
    }
}