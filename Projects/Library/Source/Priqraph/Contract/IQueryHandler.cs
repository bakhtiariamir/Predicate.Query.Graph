namespace Priqraph.Contract;

public interface IWriterHandler<TObject> : IQueryHandler<TObject> where TObject : IQueryableObject
{

}

public interface IReaderHandler<TObject> : IQueryHandler<TObject> where TObject : IQueryableObject
{

}

public interface IReWriterHandler<TObject> : IReaderHandler<TObject>, IWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface IQueryHandler<TObject> where TObject : IQueryableObject
{
    IQueryHandler<TObject>? Next
    {
        get;
        set;
    }

    void Handle(IQuery<TObject> query);

    IQueryHandler<TObject> SetNext(Func<IQueryHandler<TObject>> nextFunc);
}


public interface IEntityHandler<TObject> :IWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface IEntitiesHandler<TObject> : IWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface IRemoveHandler<TObject> : IWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface ISelectHandler<TObject> : IReWriterHandler<TObject> where TObject : IQueryableObject
{

}


public interface IWhereHandler<TObject> : IReWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface ISortHandler<TObject> : IReaderHandler<TObject> where TObject : IQueryableObject
{

}

public interface IPageHandler<TObject> : IReaderHandler<TObject> where TObject : IQueryableObject
{

}

public interface IJoinHandler<TObject> : IReWriterHandler<TObject> where TObject : IQueryableObject
{

}

public interface IGroupByHandler<TObject> : IReWriterHandler<TObject> where TObject : IQueryableObject
{

}