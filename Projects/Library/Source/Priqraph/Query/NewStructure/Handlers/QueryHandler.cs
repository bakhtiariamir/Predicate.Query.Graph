using Priqraph.Contract;
using System;
using System.Reflection.Metadata;

namespace Priqraph.Query.NewStructure.Handlers
{
    public abstract class QueryHandler<TObject> : IQueryHandler<TObject> where TObject : IQueryableObject
    {
        public IQueryHandler<TObject>? Next
        {
            get;
            set;
        }

        public abstract void Handle(IQueryObject<TObject> queryObject);

        public IQueryHandler<TObject> SetNext(Func<IQueryHandler<TObject>> nextFunc)
        {
            SetNextHandler(this, nextFunc());
            return this;
        }

        private static void SetNextHandler(IQueryHandler<TObject> handler, IQueryHandler<TObject> handlerFunc)
        {
            while (true)
            {
                if (handler.Next == null)
                    handler.Next = handlerFunc;
                else
                {
                    handler = handler.Next;
                    continue;
                }

                break;
            }
        }
    }
}
