using Priqraph.Contract;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class ColumnPredicateBuilder<TObject> : IQueryObjectPart<ColumnPredicateBuilder<TObject>, ICollection<ColumnPredicate<TObject>>> where TObject : IQueryableObject
{
    private readonly ICollection<ColumnPredicate<TObject>> _columnPredicates = new List<ColumnPredicate<TObject>>();

    private readonly Dictionary<string, string> _queryOptions = new();
    
    public static ColumnPredicateBuilder<TObject> Init() => new();

    public ColumnPredicateBuilder<TObject> Add(Expression<Func<TObject, object>> expression)
    {
        _columnPredicates.Add(new ColumnPredicate<TObject>(expression));
        return this;
    }

    public ColumnPredicateBuilder<TObject> Add(Expression<Func<TObject, IEnumerable<object>>> expression)
    {
        _columnPredicates.Add(new ColumnPredicate<TObject>(expression));
        return this;
    }

    public ColumnPredicateBuilder<TObject> AddOption(string key, string value)
    {
        _queryOptions.Add(key, value);
        return this;
    }

    public ColumnPredicateBuilder<TObject> AddOptions(IEnumerable<KeyValuePair<string, string>> options)
    {
        foreach (var keyValuePair in options)
            _queryOptions.Add(keyValuePair.Key, keyValuePair.Value);

        return this;
    }

    public ICollection<ColumnPredicate<TObject>> Return() => _columnPredicates.Count > 0 ? _columnPredicates : Add(item => item).Return();

    public ColumnPredicateBuilder<TObject> Validate() => this;

    public Dictionary<string, string> GetQueryOptions() => _queryOptions;
}