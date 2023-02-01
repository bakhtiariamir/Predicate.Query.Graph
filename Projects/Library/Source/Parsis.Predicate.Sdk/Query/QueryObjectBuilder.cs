﻿using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectBuilder<TObject> : IQueryObjectBuilder where TObject : IQueryableObject
{
    private QueryObject<TObject> _queryObjectObject;

    public QueryObjectBuilder(QueryObject<TObject> queryObject) => _queryObjectObject = queryObject;

    public static QueryObjectBuilder<TObject> Init(QueryObject<TObject> queryObject) => new(queryObject);

    public QueryObjectBuilder<TObject> SetCommand(QueryObjectCommand<TObject> objectCommand)
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        _queryObjectObject.Command = objectCommand.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSelecting(QueryObjectSelecting<TObject> objectSelecting)
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        _queryObjectObject.Columns = objectSelecting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetFiltering(QueryObjectFiltering<TObject> objectFiltering)
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        _queryObjectObject.Filters = objectFiltering.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSorting(QueryObjectSorting<TObject> objectSorting)
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        _queryObjectObject.Sorts = objectSorting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetPaging(QueryObjectPaging paging)
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        _queryObjectObject.Paging = paging.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject> Validate()
    {
        if (_queryObjectObject == null) throw new System.ArgumentNullException(); //todo
        //do
        return this;
    }

    public QueryObject<TObject> Generate() => Validate()._queryObjectObject ?? throw new System.ArgumentNullException(); //todo;
}

public interface IQueryObjectBuilder
{

}
