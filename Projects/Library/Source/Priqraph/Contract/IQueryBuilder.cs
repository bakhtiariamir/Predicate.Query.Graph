﻿namespace Priqraph.Contract;

public interface IQueryBuilder<TObject, TResult> where TObject : IQueryableObject
{
    Task<IQuery<TObject, TResult>> BuildAsync();
}