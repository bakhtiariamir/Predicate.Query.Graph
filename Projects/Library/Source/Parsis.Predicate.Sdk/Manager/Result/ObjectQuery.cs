using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Manager.Result;

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

    public abstract void UpdateParameter(params ParameterValue[] parameters);
}

public abstract class ObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> : IObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> where TObjectQuery : IObjectQuery<TParameter>
    where TQueryResult : IQueryResult
{
    public abstract TObjectQuery? GenerateResult(QueryOperationType operationType, TQueryResult query);
}
