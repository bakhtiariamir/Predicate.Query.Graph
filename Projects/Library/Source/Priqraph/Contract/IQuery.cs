using Priqraph.DataType;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Contract
{
    public interface IQuery<TObject, in TEnum> 
        where TObject : IQueryableObject 
        where TEnum : struct, IConvertible
    {
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

        IQuery<TObject, TEnum> Init(TEnum operationType, QueryProvider queryProvider,
            ICollection<Type>? objectTypeStructures = null);
    }

    public interface ISqlQuery<TObject, in TEnum> : IQuery<TObject, TEnum> 
        where TObject : IQueryableObject
        where TEnum : struct, IConvertible
    {
        DatabaseQueryOperationType DatabaseQueryOperationType
        {
            get;
            set;
        }
    }

}
