using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Query;

internal class QueryObjectBuilder<TObject> : IQueryObjectBuilder<TObject> where TObject : IQueryableObject
{
    private IQueryObject<TObject>? _queryObject;

    public QueryProvider QueryProvider
    {
        get;
        set;
    }

    public QueryObjectBuilder(QueryProvider provider)
    {
        QueryProvider = provider;
    }

    public void Init(QueryOperationType operationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) => _queryObject = QueryObject<TObject>.Init(operationType, queryProvider, objectTypeStructures);

    public IQueryObjectBuilder<TObject> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.CommandPredicates = objectCommandPredicateBuilder.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.ColumnPredicates = objectSelecting.Validate().Return();
        _queryObject.QueryOptions = objectSelecting.GetQueryOptions();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetColumns(ICollection<ColumnPredicate<TObject>> columns)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.ColumnPredicates = columns;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetFilter(FilterPredicateBuilder<TObject> objectFiltering)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.FilterPredicates = objectFiltering.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetFilter(FilterPredicate<TObject> filterPredicate)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.FilterPredicates = filterPredicate;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetSort(SortPredicateBuilder<TObject> objectSorting)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.SortPredicates = objectSorting.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.SortPredicates = sortPredicates;
        return this;
    }

    public IQueryObjectBuilder<TObject> SetPaging(PagePredicateBuilder paging)
    {
        if (_queryObject == null) throw new ArgumentNullException(); //todo
        _queryObject.PagePredicate = paging.Validate().Return();
        return this;
    }

    public IQueryObject<TObject> Generate()
    {
        if (Validate())
            return _queryObject ?? throw new ArgumentNullException();
        throw new InvalidOperationException($"Generating query for {typeof(TObject).Name} has error.");
    }

    private bool Validate()
    {
        //ToDo Implement 
        return true;
    }
}

