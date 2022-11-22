namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectBuilder<TObject, TQueryType> : IQueryObjectBuilder<TObject, TQueryType> where TObject : class where TQueryType : Enum
{
    private QueryObject<TObject, TQueryType> _object;

    private QueryObjectBuilder(TQueryType queryType)
    {
        _object = new QueryObject<TObject, TQueryType>(queryType);
    }

    public static QueryObjectBuilder<TObject, TQueryType> Init(TQueryType queryType) => new(queryType);

    public QueryObjectBuilder<TObject, TQueryType> SetSelecting(QueryObjectSelecting<TObject> objectSelecting)
    {
        _object.Columns = objectSelecting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject, TQueryType> SetFiltering(QueryObjectFiltering<TObject> objectFiltering)
    {
        _object.Filters = objectFiltering.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject, TQueryType> SetSorting(QueryObjectSorting<TObject> objectSorting)
    {
        _object.Sorts = objectSorting.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject, TQueryType> SetPaging(PageSetting<TObject> paging)
    {
        _object.Paging = paging;
        return this;
    }

    public QueryObjectBuilder<TObject, TQueryType> SetGrouping(QueryObjectGrouping<TObject> objectGrouping)
    {
        _object.Groups = objectGrouping.Validate().Return();
        return this;
    }

    public QueryObjectBuilder<TObject, TQueryType> Validate()
    {
        //do
        return this;
    }

    public QueryObject<TObject, TQueryType> Generate() => Validate()._object;

}

public interface IQueryObjectBuilder<TObject, TQueryType> where TObject : class where TQueryType : Enum
{
}
