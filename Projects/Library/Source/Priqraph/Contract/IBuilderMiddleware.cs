using Priqraph.Setup;

namespace Priqraph.Contract;

public interface IBuilderMiddleware<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{
    object Init(QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null);

    IBuilderMiddleware<TObject, TEnum> RegisterHandler<THandler>(Func<THandler> handlerFunc) where THandler : IQueryHandler<TObject, TEnum>;

    IQuery<TObject, TEnum> Build();
}

public interface IAddMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum>
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IEditMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum>
    where TObject : IQueryableObject
    where TEnum : struct,IConvertible
{

}

public interface IDeleteMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct,IConvertible
{

}

public interface IMergeMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct,IConvertible
{

}

public interface IGetMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum :struct, IConvertible
{

}

public interface ICountMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum>
    where TObject : IQueryableObject
    where TEnum : struct,IConvertible
{

}

public interface IReturnMiddleware<TObject, TEnum> : IBuilderMiddleware<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}