using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class SortPredicateBuilder<TObject> : IQueryObjectPart<SortPredicateBuilder<TObject>, ICollection<SortPredicate<TObject>>> where TObject : IQueryableObject
{
    private readonly ICollection<SortPredicate<TObject>> _sortPredicates;

    private SortPredicateBuilder()
    {
        _sortPredicates = new List<SortPredicate<TObject>>();
    }

    public static SortPredicateBuilder<TObject> Init() => new();

    public SortPredicateBuilder<TObject> Add(Expression<Func<TObject, object>> expression, DirectionType direction)
    {
        _sortPredicates.Add(new SortPredicate<TObject>(expression, direction));
        return this;
    }

    public bool HasOrder() => _sortPredicates.Count > 0;

    public ICollection<SortPredicate<TObject>> Return() => _sortPredicates;

    public Dictionary<string, string> GetQueryOptions() => new();

    public SortPredicateBuilder<TObject> Validate() => this;
}