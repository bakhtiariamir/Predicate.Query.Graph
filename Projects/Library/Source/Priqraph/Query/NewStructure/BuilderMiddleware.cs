using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.NewStructure.Middleware;
using Priqraph.Setup;

namespace Priqraph.Query.NewStructure
{
    public abstract class BuilderMiddleware<TObject, THandler> : IBuilderMiddleware<TObject> where TObject : IQueryableObject where THandler : IQueryHandler<TObject>
    {
        protected IQueryObject<TObject>? QueryObject;
        protected THandler? Handler
        {
            get;
            set;
        }

        protected abstract QueryOperationType OperationType
        {
            get;
        }

        public object Init(QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null)
        {
            QueryObject = QueryObject<TObject>.Init(OperationType, queryProvider, objectTypeStructures);
            return this;
        }

        public IBuilderMiddleware<TObject> RegisterHandler<THandler1>(Func<THandler1> handlerFunc) where THandler1 : IQueryHandler<TObject>
        {
            if (typeof(THandler) == typeof(THandler1))
            {
                var handler = handlerFunc();
                var convertedHandler = (THandler)Convert.ChangeType(handler, typeof(THandler));
                IQueryHandler<TObject> ConvertedHandlerFunc() => (IQueryHandler<TObject>)convertedHandler;
                if (Handler == null)
                   Handler = convertedHandler;
                else
                    Handler.SetNext(ConvertedHandlerFunc);
            }


            return this;
        }

        public abstract IQueryObject<TObject> Build();
    }
}

public static class MiddlewareFactory
{
    public static TMiddleware Create<TObject, TMiddleware>(QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) where TObject : IQueryableObject
        where TMiddleware : IBuilderMiddleware<TObject>
    {
        switch (typeof(TMiddleware))
        {
            case { } addMiddleware when addMiddleware == typeof(IAddMiddleware<TObject>):
                return (TMiddleware)new Add<TObject>().Init(queryProvider, objectTypeStructures);
        }

        throw new Exception(); //Error ToDo
    }
}


//Builder => Include Middleware (CommandMiddleware and QueryMiddleware =>  Add, Edit, Remove, Merge, Get, Count, Return)
//Each Middleware Includes Handlers (ICommandHandlers, IQueryHandlers => Select, Where, Join, Page, GroupBy, Sort, Add, Edit, Remove, Merge)
// 