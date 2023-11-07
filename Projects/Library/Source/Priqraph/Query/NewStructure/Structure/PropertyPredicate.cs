using Priqraph.Contract;
using Priqraph.Query.NewStructure.Handlers;
using System.Linq.Expressions;

// ReSharper disable once IdentifierTypo
namespace Priqraph.Query.NewStructure.Structure;
public class PropertyPredicate<TObject> where TObject : IQueryableObject
{
    public ICollection<Expression<Func<TObject, object>>> Expressions
    {
        get;
        set;
    }

    public PropertyPredicate()
    {
        Expressions = new List<Expression<Func<TObject, object>>>();
    }

    public void Add(Expression<Func<TObject, object>> expression) => Expressions.Add(expression);

    public void Add(ICollection<Expression<Func<TObject, object>>> expressions) => Expressions = expressions;
}



public class Select<TObject> : ReWriterHandler<TObject>, ISelectHandler<TObject> where TObject : IQueryableObject
{
    private PropertyPredicate<TObject> _predicate = new();

    public static Select<TObject> Init() => new();

    public Select<TObject> Add(Expression<Func<TObject, object>> property) // Add Single Properties
    {
        _predicate.Add(property);
        return this;
    }

    public override void Handle(IQueryObject<TObject> queryObject)
    {
        if (_predicate.Expressions is { Count: < 0 })
            _predicate.Add(item => item);

        queryObject.PropertyPredicates = _predicate;
    }
}