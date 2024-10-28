using Priqraph.DataType;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Contract;

public interface IQueryObjectBuilder<TObject, TQueryObject, in TEnum> 
    where TObject : IQueryableObject
    where TQueryObject : IQuery<TObject, TEnum>
    where TEnum : struct, IConvertible
{
    void Init(TEnum operationType, QueryProvider queryProvider, TQueryObject query, ICollection<Type>? objectTypeStructures = null);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetCommand(CommandPredicateBuilder<TObject> objectCommandPredicateBuilder);
    IQueryObjectBuilder<TObject, TQueryObject,TEnum> SetColumn(ColumnPredicateBuilder<TObject> objectSelecting);
    IQueryObjectBuilder<TObject, TQueryObject,TEnum> SetColumns(ICollection<ColumnPredicate<TObject>> columns);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetFilter(FilterPredicateBuilder<TObject> objectFiltering);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetFilter(FilterPredicate<TObject> filterPredicate);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetSort(SortPredicateBuilder<TObject> objectSorting);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetSorts(ICollection<SortPredicate<TObject>> sortPredicates);
    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetPaging(PagePredicateBuilder paging);

    IQueryObjectBuilder<TObject, TQueryObject, TEnum> SetQuery(IQueryable query);
    TQueryObject Generate();
}
