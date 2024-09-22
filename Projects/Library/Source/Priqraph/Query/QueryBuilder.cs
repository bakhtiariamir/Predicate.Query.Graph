using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Query;

public class QueryBuilder<TObject> : IQueryObjectBuilder<TObject>
    where TObject : IQueryableObject
{
    private IQuery<TObject>? _query;

    public void Init(QueryOperationType operationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) => _query = Query<TObject>.Init(operationType, queryProvider, objectTypeStructures);

    public IQueryObjectBuilder<TObject> SetQuery(IQueryable query)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.Queryable = query;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.CommandPredicates = objectCommandPredicateBuilder.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.ColumnPredicates = objectSelecting.Validate().Return();
        _query.QueryOptions = objectSelecting.GetQueryOptions();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetColumns(ICollection<ColumnPredicate<TObject>> columns)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.ColumnPredicates = columns;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetFilter(FilterPredicateBuilder<TObject> objectFiltering)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.FilterPredicates = objectFiltering.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetFilter(FilterPredicate<TObject> filterPredicate)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.FilterPredicates = filterPredicate;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetSort(SortPredicateBuilder<TObject> objectSorting)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.SortPredicates = objectSorting.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.SortPredicates = sortPredicates;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetPaging(PagePredicateBuilder paging)
    {
        if (_query == null) throw new QueryNullException("The query object is null.");
        _query.PagePredicate = paging.Validate().Return();
        return this;
    }

    public IQuery<TObject> Generate()
    {
        if (Validate())
            return _query ?? throw new ArgumentNullException();
        throw new InvalidOperationException($"Generating query for {typeof(TObject).Name} has error.");
    }

    private bool Validate() => (_query!.CommandPredicates != null && _query!.ColumnPredicates != null) || _query!.Queryable != null;
}

