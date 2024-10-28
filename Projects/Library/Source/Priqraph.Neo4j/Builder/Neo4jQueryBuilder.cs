using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Neo4j.Builder;

public class Neo4jQueryBuilder<TObject> : IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType>
    where TObject : IQueryableObject
{
    private INeo4jQuery<TObject, Neo4jQueryOperationType>? _query;

    public void Init(Neo4jQueryOperationType operationType, QueryProvider queryProvider, INeo4jQuery<TObject, Neo4jQueryOperationType> query, ICollection<Type>? objectTypeStructures = null) => _query = (INeo4jQuery<TObject, Neo4jQueryOperationType>?)query.Init(operationType, queryProvider, objectTypeStructures);
    
    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetQuery(IQueryable query)
    {
        if (_query == null) 
            throw new QueryNullException(string.Format(ExceptionCode.QueryBuilder, nameof(query), ErrorCode.IsNull), "The query object is null.");
        _query.Queryable = query;
        return this;
    }



    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder,"The query object is null.");
        _query.CommandPredicates = objectCommandPredicateBuilder.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject,INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.ColumnPredicates = objectSelecting.Validate().Return();
        _query.QueryOptions = objectSelecting.GetQueryOptions();
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetColumns(ICollection<ColumnPredicate<TObject>> columns)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.ColumnPredicates = columns;
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetFilter(FilterPredicateBuilder<TObject> objectFiltering)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.FilterPredicates = objectFiltering.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetFilter(FilterPredicate<TObject> filterPredicate)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.FilterPredicates = filterPredicate;
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetSort(SortPredicateBuilder<TObject> objectSorting)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.SortPredicates = objectSorting.Validate().Return();
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates)
    {
        if (_query == null) throw new QueryNullException(ExceptionCode.QueryBuilder, "The query object is null.");
        _query.SortPredicates = sortPredicates;
        return this;
    }

    public IQueryObjectBuilder<TObject, INeo4jQuery<TObject, Neo4jQueryOperationType>, Neo4jQueryOperationType> SetPaging(PagePredicateBuilder paging)
    {
        if (_query == null) throw new QueryNullException( ExceptionCode.QueryBuilder, "The query object is null.");
        _query.PagePredicate = paging.Validate().Return();
        return this;
    }

    public INeo4jQuery<TObject, Neo4jQueryOperationType> Generate()
    {
        if (_query == null)
            throw new NotValidException(string.Format(ExceptionCode.QueryBuilder, "query", ErrorCode.IsNull));

        if (Validate())
            return _query;

        throw new NotSupportedOperationException(string.Format(ExceptionCode.QueryBuilder, "query", ErrorCode.NotSupported));
    }

    private bool Validate() => (_query!.CommandPredicates != null && _query!.ColumnPredicates != null) || _query!.Queryable != null;
}

