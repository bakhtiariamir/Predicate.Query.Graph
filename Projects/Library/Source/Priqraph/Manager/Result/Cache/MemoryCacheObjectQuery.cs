using Priqraph.Builder.Cache;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Generator.Cache;
using Priqraph.Helper;
using Priqraph.Query;
using System.Linq.Expressions;

namespace Priqraph.Manager.Result.Cache;

public class MemoryCacheObjectQuery : ObjectQuery<BaseQueryParameter>, IMemoryCacheQuery
{
    public CacheQueryObject CacheQueryObject
    {
        get;
        set;
    }
    //Query cache
    public MemoryCacheObjectQuery(QueryOperationType queryOperationType, CacheQueryObject cacheQueryObject, ICollection<BaseQueryParameter>? parameters) : base(queryOperationType, parameters)
    {
        CacheQueryObject = cacheQueryObject;
    }

    public override void UpdateParameter(string type, params ParameterValue[] parameters) => Parameters?.ToList().ForEach(parameter =>
    {
        var newParam = parameters.FirstOrDefault(item => string.Equals(parameter.Name, item.Name, StringComparison.CurrentCultureIgnoreCase));
        if (newParam != null)
        {
        }
    });
}

public class MemoryCacheObjectQueryGenerator : IObjectQueryGenerator<BaseQueryParameter, MemoryCacheObjectQuery, CacheQueryPartCollection>
{
    public MemoryCacheObjectQuery? GenerateResult(QueryOperationType operationType, CacheQueryPartCollection query)
    {
        CacheQueryObject cacheQuery = new CacheQueryObject();
        //ICollection<BaseQueryParameter>? parameters = null;
        switch (operationType)
        {
            case QueryOperationType.GetData:
                cacheQuery = query.GetSelectQuery();
                break;
            case QueryOperationType.Add:
            case QueryOperationType.Edit:
            case QueryOperationType.Remove:
            case QueryOperationType.Merge:
                cacheQuery = query.GetCommandQuery();
                break;
        }

        return new MemoryCacheObjectQuery(operationType, cacheQuery, new List<BaseQueryParameter>());
    }
}

public class CacheQueryObject
{
    public IEnumerable<CacheCommandQueryObject>? CommandQueryObject
    {
        get;
        set;
    }

    public CacheGetDataQueryObject? GetDataQueryObject
    {
        get;
        set;
    }
}


public class CacheCommandQueryObject
{
    public object? Object
    {
        get;
        set;
    }

    public object? Key
    {
        get;
        set;
    }

    public Type? ObjectType
    {
        get;
        set;
    }
}

public class CacheGetDataQueryObject
{
    public LambdaExpression? Filter
    {
        get;
        set;
    }

    public IEnumerable<CacheSortPredicate>? Sorts
    {
        get;
        set;
    }

    public Page? Page
    {
        get;
        set;
    }
}