using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectInserting<TObject> where TObject : class
{
}

public class InsertingColumn<TObject> where TObject: class
{
    public Expression<Func<TObject, object>> Expression
    {
        get;
    }

    public object? Value
    {
        get;
    }

    public InsertingColumn(Expression<Func<TObject, object>> expression, object? value = null)
    {
        Expression = expression;
        Value = value;
    }
}

