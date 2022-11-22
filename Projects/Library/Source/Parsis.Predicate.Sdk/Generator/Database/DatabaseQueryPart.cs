using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public abstract class DatabaseQueryPart<TObject, TParameter> : IDatabaseQueryPart<TObject, TParameter> where TObject : class
{
    public abstract string Text
    {
        get;
        set;
    }

    public ICollection<TParameter> Parameters
    {
        get;
        set;
    }
    //Warn ToDo  Add IColumnPropertyInfo
    protected abstract QueryPartType QueryPartType
    {
        get;
    }

    protected DatabaseQueryPart() => Parameters = new List<TParameter>();

    public override string ToString() => Text;

    public void AddParameter(TParameter parameter) => Parameters.Add(parameter);
}

public interface IDatabaseQueryPart<TObject, TParameter> where TObject : class
{
    string Text
    {
        get;
    }

    ICollection<TParameter> Parameters
    {
        get;
    }
}