using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Manager.Result;

public abstract class ObjectQuery<TParameter> : IObjectQuery<TParameter>
{
    protected ObjectQuery(QueryOperationType queryOperationType, ICollection<TParameter>? parameters)
    {
        QueryOperationType = queryOperationType;
        Parameters = parameters;
    }

    public string? Action
    {
        get;
        set;
    }

    public QueryOperationType QueryOperationType
    {
        get;
    }

    public ICollection<TParameter>? Parameters
    {
        get;
    }

    public abstract void UpdateParameter(string type, params ParameterValue[] parameters);
}

public abstract class ObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> : IObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> where TObjectQuery : IObjectQuery<TParameter>
    where TQueryResult : IQueryResult
{
    public abstract TObjectQuery? GenerateResult(QueryOperationType operationType, TQueryResult query);
}
