using Priqraph.DataType;

namespace Priqraph.Contract;

/// <summary>
/// Main interface that external app using for create and modify queries.
/// Each type of query that want implement like database, webservice, api , ... must implement this interface.
/// </summary>
/// <typeparam name="TParameter">The type of the parameter.</typeparam>
public interface IObjectQuery<TParameter>
{
    string? Action
    {
        get;
        set;
    }


    ICollection<TParameter>? Parameters
    {
        get;
    }

    void UpdateParameter(string type, params ParameterValue[] parameters);
}