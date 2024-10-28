using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator;
using Priqraph.Generator.Database;

namespace Priqraph.Neo4j.Generator;
public class Neo4jJoinQueryFragment : DatabaseJoinQueryFragment
{
    private Neo4jJoinQueryFragment(ICollection<JoinProperty> joinPredicates)
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
            _ => throw new NotSupportedOperationException(joinProperty.JoinType.ToString(), ExceptionCode.DatabaseQueryJoiningGenerator)
        };
        //ToDo : Need PersonObjectInfo For Get id
        var joinItem = joinProperty.JoinObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException("JoinProperty");
        return $"{joinString} {joinProperty.JoinObjectInfo} AS [{joinProperty.JoinColumn}] ON [{joinProperty.JoinColumn}].[{joinItem.ColumnName}] = {joinProperty.JoinColumn.GetSelector()}.[{joinProperty.JoinColumn.ColumnName}]";
    }

    public static Neo4jJoinQueryFragment Create(params JoinProperty[] joinPredicates)
    {
        var joinFragment = new Neo4jJoinQueryFragment(joinPredicates);
        joinFragment.SetText();
        return joinFragment;
    }

    public static Neo4jJoinQueryFragment Merged(IEnumerable<Neo4jJoinQueryFragment> sqlQueries)
    {
        var joinFragment = new Neo4jJoinQueryFragment(sqlQueries.SelectMany(properties => properties.Parameter ?? new List<JoinProperty>()).DistinctBy(item => new
        {
            item.JoinColumn,
            item.JoinType
        }).ToList());
        joinFragment.SetText();

        return joinFragment;
    }
}