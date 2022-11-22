using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectJoining : IQueryObjectPart<QueryObjectJoining, ICollection<JoinPredicate>>
{
    private ICollection<JoinPredicate> _joinPredicates;

    private QueryObjectJoining()
    {
        _joinPredicates = new List<JoinPredicate>();
    }

    public static QueryObjectJoining Init() => new();

    public QueryObjectJoining Add(Expression predicate, IColumnPropertyInfo primaryKey, JoinType joinType)
    {
        _joinPredicates.Add(new JoinPredicate(predicate, primaryKey, joinType));
        return this;
    }

    public QueryObjectJoining Validate() => this;

    public ICollection<JoinPredicate> Return() => _joinPredicates;
}

public enum JoinType
{
    Inner = 1,
    Left = 2,
    Right = 3,
    Outer = 4
}

public class JoinPredicate
{
    public Expression PropertyExpression
    {
        get;
    }

    public IColumnPropertyInfo PrimaryKey
    {
        get;
        set;
    }

    public JoinType Type
    {
        get;
    }

    public JoinPredicate(Expression propertyExpression, IColumnPropertyInfo primaryKey, JoinType type)
    {
        PropertyExpression = propertyExpression;
        PrimaryKey = primaryKey;
        Type = type;
    }
}