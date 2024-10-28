using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Query;

public class QueryBuilder<TObject, TQueryObject, TEnum> : IQueryObjectBuilder<TObject, TQueryObject, TEnum>
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible  
{
    private TQueryObject? _query;

    public void Init(TEnum operationType, QueryProvider queryProvider, TQueryObject query, ICollection<Type>? objectTypeStructures = null) => _query = (TQueryObject?)query.Init(operationType, queryProvider, objectTypeStructures);
    
    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetQuery(IQueryable query)
    {
        if (_query == null) 
            throw new QueryNullException(string.Format(ExceptionCode.QueryBuilder, nameof(query), ErrorCode.IsNull), "The query object is null.");
        _query.Queryable = query;
        return this;
    }
    
    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder,"The query object is null.");
        _query.CommandPredicates = objectCommandPredicateBuilder.Validate().Return();
        return this;
    }
    
    public IQueryObjectBuilder<TObject,TQueryObject, TEnum> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.ColumnPredicates = objectSelecting.Validate().Return();
        _query.QueryOptions = objectSelecting.GetQueryOptions();
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetColumns(ICollection<ColumnPredicate<TObject>> columns)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.ColumnPredicates = columns;
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetFilter(FilterPredicateBuilder<TObject> objectFiltering)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.FilterPredicates = objectFiltering.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetFilter(FilterPredicate<TObject> filterPredicate)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.FilterPredicates = filterPredicate;
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetSort(SortPredicateBuilder<TObject> objectSorting)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.SortPredicates = objectSorting.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.SortPredicates = sortPredicates;
        return this;
    }

    public IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetPaging(PagePredicateBuilder paging)
    {
        if (_query == null) throw new QueryNullException( ExceptionCode.QueryBuilder, "The query object is null.");
        _query.PagePredicate = paging.Validate().Return();
        return this;
    }

    public TQueryObject Generate()
    {
        if (_query == null)
            throw new NotValidException(string.Format(ExceptionCode.QueryBuilder, "query", ErrorCode.IsNull));

        if (Validate())
            return _query;

        throw new NotSupportedOperationException(string.Format(ExceptionCode.QueryBuilder, "query", ErrorCode.NotSupported));
    }

    private bool Validate() => (_query!.CommandPredicates != null && _query!.ColumnPredicates != null) || _query!.Queryable != null;
}

