namespace Priqraph.Contract;
public interface IBuilder<TObject, out TMiddleware> where TObject : IQueryableObject where TMiddleware : IBuilderMiddleware<TObject>
{
    public TMiddleware Middleware
    {
        get;
    }

    IQueryObject<TObject> Build();
}

//public interface ICommandBuilder<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
//{
//    //ObjectCommand<TObject> Add(TObject obj);
//    //ObjectCommand<TObject> Add(TObject[] objects);
//    //ObjectCommand<TObject> Add<THandler>(THandler handler) where THandler : ICommandHandler<TObject>;

//    //ObjectCommand<TObject> Edit(TObject obj);
//    //ObjectCommand<TObject> Edit(TObject[] objects);
//    //ObjectCommand<TObject> Edit<THandler>(THandler handler) where THandler : ICommandHandler<TObject>;

//    //ObjectCommand<TObject> Remove(TObject obj);
//    //ObjectCommand<TObject> Remove(TObject[] objects);
//    //ObjectCommand<TObject> Remove<THandler>(THandler handler) where THandler : ICommandHandler<TObject>;
//}

//public interface IQueryBuilder<TObject> : IBuilderMiddleware<TObject> where TObject : IQueryableObject
//{
//    //ICollection<QueryColumn<TObject>> Select(Func<TObject, object> property);

//    //ICollection<QueryColumn<TObject>> Select(Func<TObject, IEnumerable<object>> properties);

//    //FilterPredicate<TObject> Where(Func<TObject, bool> clause);
//    //ICollection<FilterPredicate<TObject>> Where(IEnumerable<Func<TObject, bool>> clauses);

//    //SortPredicate<TObject> Sort(Func<TObject, object> property, DirectionType direction);
//    //ICollection<SortPredicate<TObject>> Sort(IEnumerable<(Func<TObject, object> Property, DirectionType Direction)> properties);

//    //PagePredicate Page(int skip, int take);
//}

