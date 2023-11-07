using Priqraph.Contract;

namespace Priqraph.Manager.Result;

public abstract class ObjectQuery<TParameter> : IObjectQuery<TParameter>
{
    protected ObjectQuery(ICollection<TParameter>? parameters)
    {
        Parameters = parameters;
    }

    public string? Action
    {
        get;
        set;
    }

    public ICollection<TParameter>? Parameters
    {
        get;
    }

    public abstract void UpdateParameter(string type, params ParameterValue[] parameters);
}