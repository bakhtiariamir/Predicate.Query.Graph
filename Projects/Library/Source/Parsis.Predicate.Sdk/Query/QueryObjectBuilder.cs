using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;
public class QueryObjectBuilder<TObject> : IQueryObjectBuilder<TObject> where TObject : class
{
    private QueryObject<TObject> _object;

    private QueryObjectBuilder(QueryType queryType)
    {
        _object = new QueryObject<TObject>(queryType);
    }

    public static QueryObjectBuilder<TObject> Init(QueryType queryType) => new(queryType);

    public QueryObjectBuilder<TObject> SetSelecting(QueryObjectSelecting<TObject> objectSelecting)
    {
        _object.Columns = objectSelecting.Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetFiltering(QueryObjectFiltering<TObject> objectFiltering)
    {
        _object.Filters = objectFiltering.Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetSorting(QueryObjectSorting<TObject> objectSorting)
    {
        _object.Sorts = objectSorting.Return();
        return this;
    }

    public QueryObjectBuilder<TObject> SetPaging(PageSetting<TObject> paging)
    {
        _object.Paging = paging;
        return this;
    }

    public QueryObjectBuilder<TObject> SetGrouping(QueryObjectGrouping<TObject> objectGrouping)
    {
        _object.Groups = objectGrouping.Return();
        return this;
    }

    public QueryObjectBuilder<TObject> Validate()
    {
        //do
        return this;
    }

    public QueryObject<TObject> Generate() => _object;

}

public interface IQueryObjectBuilder<TObject> where TObject : class
{
}
