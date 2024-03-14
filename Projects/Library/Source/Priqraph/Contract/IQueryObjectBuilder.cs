using Priqraph.DataType;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Contract;

public interface IQueryObjectBuilder<TObject> where TObject : IQueryableObject
{
    void Init(QueryOperationType operationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null);
    IQueryObjectBuilder<TObject> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder);
    IQueryObjectBuilder<TObject> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting);
    IQueryObjectBuilder<TObject> SetColumns(ICollection<ColumnPredicate<TObject>> columns);
    IQueryObjectBuilder<TObject> SetFilter(FilterPredicateBuilder<TObject> objectFiltering);
    IQueryObjectBuilder<TObject> SetFilter(FilterPredicate<TObject> filterPredicate);
    IQueryObjectBuilder<TObject> SetSort(SortPredicateBuilder<TObject> objectSorting);
    IQueryObjectBuilder<TObject> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates);
    IQueryObjectBuilder<TObject> SetPaging(PagePredicateBuilder paging);

    IQueryObjectBuilder<TObject> SetQuery(IQueryable query);
    IQueryObject<TObject> Generate();
}
