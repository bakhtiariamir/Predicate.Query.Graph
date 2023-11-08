using Priqraph.Contract;

namespace Priqraph.Generator.Database;

public abstract class QueryFragment<TParameter> : IQueryFragment<TParameter> where TParameter : class
{
    public string? Text
    {
        get;
        protected set;
    }

    public TParameter? Parameter
    {
        get;
        set;
    }

    public override string? ToString() => Text;
}
