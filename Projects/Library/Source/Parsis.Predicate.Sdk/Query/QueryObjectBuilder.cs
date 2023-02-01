using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectBuilder<TObject> : IQueryObjectBuilder where TObject : IQueryableObject
{
    private QueryObject<TObject>? _object;

    public static QueryObjectBuilder<TObject> Init() => new();

    public QueryObjectBuilder<TObject> SetCommand(QueryObjectCommand<TObject> objectCommand)
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        _object.Command = objectCommand.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSelecting(QueryObjectSelecting<TObject> objectSelecting)
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        _object.Columns = objectSelecting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetFiltering(QueryObjectFiltering<TObject> objectFiltering)
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        _object.Filters = objectFiltering.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSorting(QueryObjectSorting<TObject> objectSorting)
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        _object.Sorts = objectSorting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetPaging(QueryObjectPaging paging)
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        _object.Paging = paging.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> Validate()
    {
        if (_object == null) throw new System.ArgumentNullException(); //todo
        //do
        return this;
    }

    public QueryObject<TObject> Generate() => Validate()._object ?? throw new System.ArgumentNullException(); //todo;
}

public interface IQueryObjectBuilder
{

}
