﻿using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Contract;

public interface IQueryOperation<TObject, TResult> where TObject : IQueryableObject
{
    Task<TResult> RunAsync(QueryObject<TObject> queryObject);

    void Init(QueryOperationType queryOperationType);

    QueryObjectBuilder<TObject>? QueryBuilder
    {
        get;
    }
}
