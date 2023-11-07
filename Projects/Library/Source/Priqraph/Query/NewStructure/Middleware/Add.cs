using Priqraph.Contract;
using Priqraph.DataType;

namespace Priqraph.Query.NewStructure.Middleware
{
    public class Add<TObject> : BuilderMiddleware<TObject, IReWriterHandler<TObject>>, IAddMiddleware<TObject> where TObject : IQueryableObject
    {
        protected override QueryOperationType OperationType => QueryOperationType.Add;

        public override IQueryObject<TObject> Build()
        {
            if (QueryObject == null)
                throw new System.Exception(); // ToDo
            var handler = Handler;
            while (true)
            {
                if (handler != null)
                {
                    handler.Handle(QueryObject);
                    if (handler.Next != null)
                        handler = (IReWriterHandler<TObject>)handler.Next;
                    continue;
                }

                break;
            }
            return QueryObject;
        }

    }
}
