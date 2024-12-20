﻿using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Manager.Result
{
    public abstract class ObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> : IObjectQueryGenerator<TParameter, TObjectQuery, TQueryResult> where TObjectQuery : IObjectQuery<TParameter>
        where TQueryResult : IQueryResult
    {
        public abstract TObjectQuery? GenerateResult(DatabaseQueryOperationType operationType, TQueryResult query);
    }
}
