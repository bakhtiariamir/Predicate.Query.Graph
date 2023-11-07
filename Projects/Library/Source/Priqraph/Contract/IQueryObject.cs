using Priqraph.DataType;
using Priqraph.Query.Predicates;

namespace Priqraph.Contract
{
    public interface IQueryObject<TObject> where TObject : IQueryableObject
    {
        QueryOperationType QueryOperationType
        {
            get;
            set;
        }


        CommandPredicate<TObject>? CommandPredicates
        {
            get;
            set;
        }

        ICollection<ColumnPredicate<TObject>>? ColumnPredicates
        {
            get;
            set;
        }

        ICollection<JoinPredicate>? JoinPredicates
        {
            get;
            set;
        }

        Query.Predicates.FilterPredicate<TObject>? FilterPredicates
        {
            get;
            set;
        }

        ICollection<SortPredicate<TObject>>? SortPredicates
        {
            get;
            set;
        }

        PagePredicate? PagePredicate
        {
            get;
            set;
        }

        Dictionary<string, string> QueryOptions
        {
            get;
            set;
        }

        ICollection<Type> ObjectTypeStructures
        {
            get;
            set;
        }
    }
}
