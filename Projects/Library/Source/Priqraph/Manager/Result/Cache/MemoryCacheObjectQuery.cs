using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Generator.Cache;
using Priqraph.Query.Builders;
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
    public MemoryCacheObjectQuery(CacheQueryObject cacheQueryObject, ICollection<BaseQueryParameter>? parameters) : base(parameters)
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