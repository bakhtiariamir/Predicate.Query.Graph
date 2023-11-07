using Priqraph.Contract;
using Priqraph.Setup;

namespace Priqraph.Query.NewStructure;
internal class Builder<TObject, TMiddleware> : IBuilder<TObject, TMiddleware> where TObject : IQueryableObject
where TMiddleware : IBuilderMiddleware<TObject>
{
    private readonly ICacheInfoCollection _cacheInfoCollection;
    public TMiddleware Middleware
    {
        get;
    }

    private Builder(QueryProvider queryProvider, ICacheInfoCollection cacheInfoCollection, Type[]? objectTypeStructures = null)
    {
        _cacheInfoCollection = cacheInfoCollection;
        Middleware = MiddlewareFactory.Create<TObject, TMiddleware>(queryProvider, objectTypeStructures);
    }

    public static Builder<TObject, TMiddleware> Init(QueryProvider provider, ICacheInfoCollection cacheInfoCollection) => new(provider, cacheInfoCollection);


    public IQueryObject<TObject> Build()
    {
        if (Validate(typeof(TMiddleware)))
            return Middleware.Build();

        throw new InvalidOperationException($"Cannot build, Query or Command not defined.");
    }


    private bool Validate(Type middlewareType)
    {
        return true;
    }
}














internal enum BuilderType
{
    Command,
    Query
}