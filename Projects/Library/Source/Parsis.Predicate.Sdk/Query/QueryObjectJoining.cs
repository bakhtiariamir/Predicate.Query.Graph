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

    public QueryObjectJoining Add(Expression predicate, JoinType joinType, int order)
    {
        _joinPredicates.Add(new JoinPredicate(predicate, joinType, order));
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

    public JoinType Type
    {
        get;
    }

    public int Order
    {
        get;
    }

    public JoinPredicate(Expression propertyExpression, JoinType type, int order)
    {
        PropertyExpression = propertyExpression;
        Type = type;
        Order = order;
    }
}