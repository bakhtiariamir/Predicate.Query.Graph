using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectObjectSelecting<TObject> : IQueryObjectPart<QueryObjectObjectSelecting<TObject>> where TObject : class
{
    private IList<QueryColumn<TObject>> _columns;

    private QueryObjectObjectSelecting() => _columns = new List<QueryColumn<TObject>>();

    public static QueryObjectObjectSelecting<TObject> Init() => new();

    public QueryObjectObjectSelecting<TObject> Add(Expression<Func<TObject, object>> expression)
    {
        _columns.Add(new QueryColumn<TObject>(expression));
        return this;
    }

    public IList<QueryColumn<TObject>> Return() => _columns;

    public QueryObjectObjectSelecting<TObject> Validation() => this;
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
