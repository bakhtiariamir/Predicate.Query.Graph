using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectSelecting<TObject> : IQueryObjectPart<QueryObjectSelecting<TObject>> where TObject : class
{
    private IList<QueryColumn<TObject>> _columns;

    private QueryObjectSelecting() => _columns = new List<QueryColumn<TObject>>();

    public static QueryObjectSelecting<TObject> Init() => new();

    public QueryObjectSelecting<TObject> Add(Expression<Func<TObject, object>> expression)
    {
        _columns.Add(new QueryColumn<TObject>(expression));
        return this;
    }

    public IList<QueryColumn<TObject>> Return() => _columns;

    public QueryObjectSelecting<TObject> Validation() => this;
}


public class QueryColumn<TObject> where TObject : class
{
    public Expression<Func<TObject, object>> Expression
    {
        get;
    }

    public QueryColumn(Expression<Func<TObject, object>> expression)
    {
        Expression = expression;
    }
}
