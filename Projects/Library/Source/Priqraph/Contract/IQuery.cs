using Priqraph.DataType;
using Priqraph.Query.Predicates;

namespace Priqraph.Contract
{
    public interface IQuery<TObject> where TObject : IQueryableObject
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

        FilterPredicate<TObject>? FilterPredicates
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

        IQueryable? Queryable
        {
            get;
            set;
        }
    }
}
