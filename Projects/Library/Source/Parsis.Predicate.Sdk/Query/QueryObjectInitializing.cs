using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using System.Data;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectInitializing<TObject> : IQueryObjectPart<QueryObjectInitializing<TObject>, ObjectInitializing<TObject>> where TObject : IQueryableObject
{
    private ObjectInitializing<TObject> _initializing;
    public QueryInitializeType InitializeType
    {
        get;
        set;
    }

    private QueryObjectInitializing(QueryInitializeType initializeType)
    {

        InitializeType = initializeType;
        _initializing = new ObjectInitializing<TObject>();
    }

    public static QueryObjectInitializing<TObject> Init(QueryInitializeType initializeType) => new(initializeType);

    public QueryObjectInitializing<TObject> Add(Expression<Func<TObject, object>> expression, object? value = null)
    {
        _initializing.Columns.Add(new InitializeColumn<TObject>(expression, value));
        return this;
    }

    public QueryObjectInitializing<TObject> SetDataTable(DataTable dataTable)
    {
        _initializing.DataTable = dataTable;
        return this;
    }

    public QueryObjectInitializing<TObject> SetObject(TObject obj)
    {
        _initializing.Value = obj;
        return this;
    }

    public ObjectInitializing<TObject> Return() => _initializing;


    //ToDo :1) If is DaaTable and dataTable is empty throw exception
    //ToDo :2) If is Object and object is empty throw exception
    //ToDo :3) At the same time just one state can has value 
    public QueryObjectInitializing<TObject> Validate() => this;
}


public class ObjectInitializing<TObject> where TObject : IQueryableObject
{
    public IList<InitializeColumn<TObject>> Columns
    {
        get;
    }

    public DataTable? DataTable
    {
        get;
        set;
    }

    public TObject? Value
    {
        get;
        set;
    }

    public ObjectInitializing()
    {
        Columns = new List<InitializeColumn<TObject>>();
    }

}

public class InitializeColumn<TObject> where TObject : IQueryableObject
{
    public Expression<Func<TObject, object>> Expression
    {
        get;
    }

    public object? Value
    {
        get;
    }

    public InitializeColumn(Expression<Func<TObject, object>> expression, object? value = null)
    {
        Expression = expression;
        Value = value;
    }
}

