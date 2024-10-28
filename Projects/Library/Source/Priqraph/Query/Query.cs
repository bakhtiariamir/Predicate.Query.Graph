using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Query;

internal class Query<TObject, TEnum> : IQuery<TObject, TEnum> 
    where TObject : IQueryableObject
    where TEnum : struct, IConvertible  
{
    public DatabaseQueryOperationType DatabaseQueryOperationType
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

    public IQueryable? Queryable
    {
        get;
        set;
    }

    public IQuery<TObject, TEnum> Init(TEnum operationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) => throw new NotImplementedException();

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

    private Query(DatabaseQueryOperationType databaseQueryOperationType)
    {
        DatabaseQueryOperationType = databaseQueryOperationType;
        ObjectTypeStructures.Add(typeof(Type));
    }

    private Query(DatabaseQueryOperationType databaseQueryOperationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null)
    {
        QueryProvider = queryProvider;
        DatabaseQueryOperationType = databaseQueryOperationType;
        ObjectTypeStructures = objectTypeStructures?.ToList() ?? new List<Type>();
        //FIXME - What is this
        if (objectTypeStructures is not null && !objectTypeStructures.Contains(typeof(Type)))
            ObjectTypeStructures.Add(typeof(Type));
    }

    public IQuery<TObject, TEnum> Init(DatabaseQueryOperationType databaseQueryOperationType, QueryProvider queryProvider, ICollection<Type>? objectTypeStructures = null) => new Query<TObject, TEnum>(databaseQueryOperationType, queryProvider, objectTypeStructures);
}
