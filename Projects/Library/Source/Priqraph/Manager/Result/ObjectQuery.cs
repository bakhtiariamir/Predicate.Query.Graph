using Priqraph.Contract;

namespace Priqraph.Manager.Result;

public abstract class ObjectQuery<TParameter>(ICollection<TParameter>? parameters) : IObjectQuery<TParameter>
{
    public string? Action
    {
        get;
        set;
    }

    public ICollection<TParameter>? Parameters
    {
        get;
    } = parameters;

    public abstract void UpdateParameter(string type, params ParameterValue[] parameters);
}