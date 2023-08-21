using Dynamitey.DynamicObjects;
using Priqraph.DataType;
using System.Data.SqlClient;

namespace Priqraph.Contract;

public interface ISqlQuery : IObjectQuery<SqlParameter>
{
    public string Phrase
    {
        get;
    }
}

public interface IMemoryCacheQuery : IObjectQuery<BaseQueryParameter>
{
}


public class BaseQueryParameter
{
    public string Name
    {
        get;
    }

    public object? Value
    {
        get;
        set;
    }

    public BaseQueryParameter(string name, object? value)
    {
        Name = name;
        Value = value;
    }
}

public interface IObjectQuery<TParameter>
{
    string? Action
    {
        get;
        set;
    }

    QueryOperationType QueryOperationType
    {
        get;
    }

    ICollection<TParameter>? Parameters
    {
        get;
    }

    void UpdateParameter(string type, params ParameterValue[] parameters);
}

public class ParameterValue
{
    public string? Name
    {
        get;
        set;
    }

    public object? Value
    {
        get;
        set;
    }
}
