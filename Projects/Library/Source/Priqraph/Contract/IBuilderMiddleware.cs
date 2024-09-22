using Priqraph.Setup;

namespace Priqraph.Contract;

public interface IBuilderMiddleware<TObject> where TObject : IQueryableObject
{
    object Init(QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null);

    IBuilderMiddleware<TObject> RegisterHandler<THandler>(Func<THandler> handlerFunc) where THandler : IQueryHandler<TObject>;

    IQuery<TObject> Build();
}

public interface IAddMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface IEditMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface IDeleteMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface IMergeMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface IGetMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface ICountMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}

public interface IReturnMiddleware<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
{

}