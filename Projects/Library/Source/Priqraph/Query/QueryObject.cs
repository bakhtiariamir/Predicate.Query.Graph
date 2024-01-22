using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Query;

public class QueryObject<TObject> : IQueryObject<TObject> where TObject : IQueryableObject
{
    public QueryOperationType QueryOperationType
    {
        get;
        set;
    }

    public QueryProvider QueryProvider
    {
        get;
        set;
    }

    public ICollection<ColumnPredicate<TObject>>? ColumnPredicates
    {
        get;
        set;
    }

    public CommandPredicate<TObject>? CommandPredicates
    {
        get;
        set;
    }

    public ICollection<JoinPredicate>? JoinPredicates
    {
        get;
        set;
    }

    public FilterPredicate<TObject>? FilterPredicates
    {
        get;
        set;
    }

    public ICollection<SortPredicate<TObject>>? SortPredicates
    {
        get;
        set;
    }

    public PagePredicate? PagePredicate
    {
        get;
        set;
    }


    public Dictionary<string, string> QueryOptions
    {
        get;
        set;
    } = new();

    public ICollection<Type> ObjectTypeStructures
    {
        get;
        set;
    } = new List<Type>();

    private QueryObject(QueryOperationType queryOperationType)
    {
        QueryOperationType = queryOperationType;
        ObjectTypeStructures.Add(typeof(Type));
    }

    private QueryObject(QueryOperationType queryOperationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null)
    {
        QueryProvider = queryProvider;
        QueryOperationType = queryOperationType;
        ObjectTypeStructures = objectTypeStructures?.ToList() ?? new List<Type>();
        //FIXME - What is this
        // if (objectTypeStructures is not null && !objectTypeStructures.Contains(typeof(Type)))
        //     ObjectTypeStructures.Add(typeof(Type));
    }

    public static QueryObject<TObject> Init(QueryOperationType queryOperationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) => new(queryOperationType, queryProvider, objectTypeStructures);
}
