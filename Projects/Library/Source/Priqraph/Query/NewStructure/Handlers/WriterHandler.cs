using Priqraph.Contract;

namespace Priqraph.Query.NewStructure.Handlers;

public abstract class WriterHandler<TObject> : QueryHandler<TObject>, IWriterHandler<TObject> where TObject : IQueryableObject
{
}


public abstract class ReaderHandler<TObject> : QueryHandler<TObject>, IReaderHandler<TObject> where TObject : IQueryableObject
{
}

public abstract class ReWriterHandler<TObject> : QueryHandler<TObject>, IReWriterHandler<TObject> where TObject : IQueryableObject
{

}
