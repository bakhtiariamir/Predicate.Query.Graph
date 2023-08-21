﻿using Priqraph.Contract;
using System.Linq.Expressions;

namespace Priqraph.Query;


//cache query 
// => https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
public class QueryObjectSelecting<TObject> : IQueryObjectPart<QueryObjectSelecting<TObject>, ICollection<QueryColumn<TObject>>> where TObject : IQueryableObject
{
    private ICollection<QueryColumn<TObject>> _columns;

    private Dictionary<string, string> _queryOptions = new();

    private QueryObjectSelecting() => _columns = new List<QueryColumn<TObject>> { new(item => item)};

    public static QueryObjectSelecting<TObject> Init() => new();

    public QueryObjectSelecting<TObject> Add(Expression<Func<TObject, object>> expression)
    {
        _columns.Add(new QueryColumn<TObject>(expression));
        return this;
    }

    public QueryObjectSelecting<TObject> Add(Expression<Func<TObject, IEnumerable<object>>> expression)
    {
        _columns.Add(new QueryColumn<TObject>(expression));
        return this;
    }

    public QueryObjectSelecting<TObject> AddOption(string key, string value)
    {
        _queryOptions.Add(key, value);
        return this;
    }

    public QueryObjectSelecting<TObject> AddOptions(IEnumerable<KeyValuePair<string, string>> options)
    {
        foreach (var keyValuePair in options)
        {
            _queryOptions.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return this;
    }

    public ICollection<QueryColumn<TObject>> Return() => _columns.Count > 0 ? _columns : Add(item => item).Return();

    public QueryObjectSelecting<TObject> Validate() => this;

    public Dictionary<string, string> GetQueryOptions() => _queryOptions;
}

public class QueryColumn<TObject>
{
    public Expression<Func<TObject, object>>? Expression
    {
        get;
    }

    public Expression<Func<TObject, IEnumerable<object>>>? Expressions
    {
        get;
    }

    public IEnumerable<QueryObjectPartIssue>? Issues
    {
        get;
        set;
    } = new List<QueryObjectPartIssue>();

    public QueryColumn(Expression<Func<TObject, object>>? expression)
    {
        Expression = expression;
    }

    public QueryColumn(Expression<Func<TObject, IEnumerable<object>>>? expressions)
    {
        Expressions = expressions;
    }
}

public class QueryObjectPartIssue
{
    public string Code
    {
        get;
    }

    public string Description
    {
        get;
    }

    public QueryObjectPartIssue(string code, string description)
    {
        Code = code;
        Description = description;
    }
}