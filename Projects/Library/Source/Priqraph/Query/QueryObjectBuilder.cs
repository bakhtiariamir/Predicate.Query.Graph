using Priqraph.Contract;

namespace Priqraph.Query;

public class QueryObjectBuilder<TObject> : IQueryObjectBuilder where TObject : IQueryableObject
{
    private QueryObject<TObject> _queryObjectObject;

    private QueryObjectBuilder(QueryObject<TObject> queryObject) => _queryObjectObject = queryObject;

    public static QueryObjectBuilder<TObject> Init(QueryObject<TObject> queryObject) => new(queryObject);

    public QueryObjectBuilder<TObject> SetCommand(QueryObjectCommand<TObject> objectCommand)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Command = objectCommand.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSelecting(QueryObjectSelecting<TObject> objectSelecting)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Columns = objectSelecting.Validate().Return();
        _queryObjectObject.QueryOptions = objectSelecting.GetQueryOptions();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSelecting(ICollection<QueryColumn<TObject>> columns)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Columns = columns;
        return this;
    }

    public QueryObjectBuilder<TObject> SetFiltering(QueryObjectFiltering<TObject> objectFiltering)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Filters = objectFiltering.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetFilterPredicate(FilterPredicate<TObject> filterPredicate)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Filters = filterPredicate;
        return this;
    }

    public QueryObjectBuilder<TObject> SetSorting(QueryObjectSorting<TObject> objectSorting)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Sorts = objectSorting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSortPredicate(ICollection<SortPredicate<TObject>> sortPredicates)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Sorts = sortPredicates;
        return this;
    }

    public QueryObjectBuilder<TObject> SetPaging(QueryObjectPaging paging)
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        _queryObjectObject.Paging = paging.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> Validate()
    {
        if (_queryObjectObject == null) throw new ArgumentNullException(); //todo
        //do
        return this;
    }

    public QueryObject<TObject> Generate() => Validate()._queryObjectObject ?? throw new ArgumentNullException(); //todo;
}

public interface IQueryObjectBuilder
{
}
