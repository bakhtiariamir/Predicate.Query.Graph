﻿namespace Priqraph.Contract;

public interface IQueryObjectPart<out TQueryPart, out TResult>
{
    TQueryPart Validate();

    TResult Return();

    Dictionary<string, string> GetQueryOptions();
}
