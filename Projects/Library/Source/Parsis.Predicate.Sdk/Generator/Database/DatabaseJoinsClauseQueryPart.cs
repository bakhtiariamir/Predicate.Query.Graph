using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Generator.Database;

public class DatabaseJoinsClauseQueryPart : DatabaseQueryPart<ICollection<JoinPredicate>>
{
    private string? _text;

    public override string? Text
    {
        get => _text;
        set => _text = value;
    }

    DatabaseJoinsClauseQueryPart(ICollection<JoinPredicate> joinPredicates)
    {
        Parameter = joinPredicates;
    }

    DatabaseJoinsClauseQueryPart(JoinPredicate joinPredicate)
    {
        Parameter = new[] {joinPredicate};
        SetText();
    }

    private static string SetColumnName(IColumnPropertyInfo item) => $"{item.Alias ?? item.GetCombinedAlias()}.{item.ColumnName}";

    private void SetText() => _text = string.Join(" ", Parameter.Select(SetJoinText));

    private string SetJoinText(JoinPredicate joinPredicate)
    {
        var joinString = joinPredicate.JoinType switch {
            JoinType.Inner => "INNER JOIN",
            JoinType.Left => "LEFT JOIN",
            JoinType.Right => "RIGHT JOIN",
            JoinType.Outer => "OUTER JOIN",
            _ => throw new NotSupported(joinPredicate.JoinType.ToString(), ExceptionCode.DatabaseQueryJoiningGenerator)
        };
        //ToDo : Need PersonObjectInfo For Get id
        var joinProperty = joinPredicate.JoinObjectInfo.PropertyInfos.FirstOrDefault(item => item.IsPrimaryKey);
        return $"{joinString} {joinPredicate.JoinObjectInfo} AS [{joinPredicate.JoinColumn}] ON [{joinPredicate.JoinColumn}].[{joinProperty.ColumnName}] = {joinProperty.GetSelector()}.[{joinPredicate.JoinColumn.ColumnName}]";
    }

    public static DatabaseJoinsClauseQueryPart Create(params JoinPredicate[] joinPredicates) => new(joinPredicates);

    public static DatabaseJoinsClauseQueryPart Merged(params DatabaseJoinsClauseQueryPart[] sqlClauses) => new(sqlClauses.SelectMany(properties => properties.Parameter).ToList());

    public static DatabaseJoinsClauseQueryPart Merged(IEnumerable<DatabaseJoinsClauseQueryPart> sqlQueries)
    {
        var column = new DatabaseJoinsClauseQueryPart(sqlQueries.SelectMany(properties => properties.Parameter).DistinctBy(item => new {
            JoinColumn = item.JoinColumn,
            item.JoinType
        }).ToList());
        column.SetText();

        return column;
    }
}

public class JoinPredicate
{
    public IColumnPropertyInfo JoinColumn
    {
        get;
    }

    public JoinType JoinType
    {
        get;
    }

    public IDatabaseObjectInfo JoinObjectInfo
    {
        get;
    }

    public JoinPredicate(IColumnPropertyInfo joinColumn, JoinType joinType, IDatabaseObjectInfo joinObjectInfo)
    {
        JoinColumn = joinColumn;
        JoinType = joinType;
        JoinObjectInfo = joinObjectInfo;
    }
}
