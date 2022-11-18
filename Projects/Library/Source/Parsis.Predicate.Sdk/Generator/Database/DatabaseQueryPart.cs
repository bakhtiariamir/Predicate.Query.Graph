using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public abstract class DatabaseQueryPart<TObject, TParameter> : IDatabaseQueryPart<TObject, TParameter> where TObject : class
{
    public abstract string Text
    {
        get;
        set;
    }

    public IEnumerable<TParameter> Parameters
    {
        get;
        set;
    }

    protected abstract QueryPartType QueryPartType
    {
        get;
    }

    protected DatabaseQueryPart() => Parameters = new List<TParameter>();

    public override string ToString() => Text;
}

public interface IDatabaseQueryPart<TObject, out TParameter> where TObject : class
{
    string Text
    {
        get;
    }

    IEnumerable<TParameter> Parameters
    {
        get;
    }
}