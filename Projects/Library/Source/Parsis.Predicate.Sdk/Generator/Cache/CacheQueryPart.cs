namespace Parsis.Predicate.Sdk.Generator.Cache;

public abstract class CacheQueryPart<TParameter> : ICacheQueryPart<TParameter> where TParameter : class
{
    public TParameter Parameter
    {
        get;
        set;
    }
}

public interface ICacheQueryPart<out TParameter> : ICacheQueryPart where TParameter : class
{
    TParameter Parameter
    {
        get;
    }
}

public interface ICacheQueryPart
{
    //IQueryable Queryable
    //{
    //    get;
    //    set;
    //}
}
