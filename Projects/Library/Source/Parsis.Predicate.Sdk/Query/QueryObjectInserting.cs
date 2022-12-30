using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using System.Data;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectInserting<TObject> : IQueryObjectPart<QueryObjectInserting<TObject>, ObjectInitializing<TObject>> where TObject : IQueryableObject
{
    private ObjectInitializing<TObject> _inserting;
    public CommandValueType CommandValueType
    {
        get;
        set;
    }

    private QueryObjectInserting(CommandValueType commandValueType)
    {

        CommandValueType = commandValueType;
        _inserting = new ObjectInitializing<TObject>();
    }

    public static QueryObjectInserting<TObject> Init(CommandValueType initializeType) => new(initializeType);

    public QueryObjectInserting<TObject> Set(Expression<Func<TObject>> expression)
    {
        //_inserting.Columns.Add(new InitializeObject<TObject>(expression));
        return this;
    }


    public QueryObjectInserting<TObject> Set(Expression<Func<TObject, object>> expression)
    {
        _inserting.Columns.Add(new InitializeObject<TObject>(expression));
        return this;
    }

    public QueryObjectInserting<TObject> SetDataTable(DataTable dataTable)
    {
        _inserting.DataTable = dataTable;
        return this;
    }

    public QueryObjectInserting<TObject> SetObject(TObject obj)
    {
        _inserting.Value = obj;
        return this;
    }

    public QueryObjectInserting<TObject> Validate() => this;

    public ObjectInitializing<TObject> Return() => _inserting;

}

public class ObjectInitializing<TObject> where TObject : IQueryableObject
{
    public ICollection<InitializeObject<TObject>> Columns
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
        Columns = new List<InitializeObject<TObject>>();
    }

}

public class InitializeObject<TObject> where TObject : IQueryableObject
{
    public Expression<Func<TObject, object>> Predicate
    {
        get;
    }

    public object? Value
    {
        get;
    }

    public InitializeObject(Expression<Func<TObject, object>> predicate, object? value = null)
    {
        Predicate = predicate;
        Value = value;
    }
}

