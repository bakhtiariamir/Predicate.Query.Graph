namespace Parsis.Predicate.Sdk.Generator.Database;

public abstract class DatabaseQueryPart<TParameter> : IDatabaseQueryPart<TParameter> where TParameter : class
{
    public abstract string? Text
    {
        get;
        set;
    }

    public TParameter Parameter
    {
        get;
        set;
    }

    public override string? ToString() => Text;
}

public interface IDatabaseQueryPart<out TParameter> : IDatabaseQueryPart where TParameter : class
{
    TParameter Parameter
    {
        get;
    }
}

public interface IDatabaseQueryPart
{
    string? Text
    {
        get;
    }
}
