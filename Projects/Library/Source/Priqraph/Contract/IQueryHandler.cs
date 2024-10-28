namespace Priqraph.Contract;

public interface IWriterHandler<TObject, TEnum> : IQueryHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IReaderHandler<TObject, TEnum> : IQueryHandler<TObject,TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IReWriterHandler<TObject, TEnum> : IReaderHandler<TObject, TEnum>, IWriterHandler<TObject, TEnum>
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IQueryHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{
    IQueryHandler<TObject, TEnum>? Next
    {
        get;
        set;
    }

    void Handle(IQuery<TObject, TEnum> query);

    IQueryHandler<TObject, TEnum> SetNext(Func<IQueryHandler<TObject, TEnum>> nextFunc);
}


public interface IEntityHandler<TObject, TEnum> :IWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IEntitiesHandler<TObject, TEnum> : IWriterHandler<TObject, TEnum>
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IRemoveHandler<TObject, TEnum> : IWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface ISelectHandler<TObject, TEnum> : IReWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}


public interface IWhereHandler<TObject, TEnum> : IReWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface ISortHandler<TObject, TEnum> : IReaderHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IPageHandler<TObject, TEnum> : IReaderHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IJoinHandler<TObject, TEnum> : IReWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}

public interface IGroupByHandler<TObject, TEnum> : IReWriterHandler<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible
{

}