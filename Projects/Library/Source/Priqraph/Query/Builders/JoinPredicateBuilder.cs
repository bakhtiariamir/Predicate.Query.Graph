using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class JoinPredicateBuilder : IQueryObjectPart<JoinPredicateBuilder, ICollection<JoinPredicate>>
{
    private readonly ICollection<JoinPredicate> _joinPredicates;

    private JoinPredicateBuilder()
    {
        _joinPredicates = new List<JoinPredicate>();
    }

    public static JoinPredicateBuilder Init() => new();

    public JoinPredicateBuilder Add(Expression predicate, JoinType joinType, int order)
    {
        _joinPredicates.Add(new JoinPredicate(predicate, joinType, order));
        return this;
    }

    public JoinPredicateBuilder Validate() => this;

    public ICollection<JoinPredicate> Return() => _joinPredicates;

    public Dictionary<string, string> GetQueryOptions() => new();
}