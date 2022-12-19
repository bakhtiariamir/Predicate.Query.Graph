using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectGrouping<TObject> : IQueryObjectPart<QueryObjectGrouping<TObject>, ICollection<GroupPredicate<TObject>>> where TObject : IQueryableObject
{
    private readonly ICollection<GroupPredicate<TObject>> _groupPredicates;

    private QueryObjectGrouping()
    {
        _groupPredicates = new List<GroupPredicate<TObject>>();
    }

    public static QueryObjectGrouping<TObject> Init() => new();

    public QueryObjectGrouping<TObject> Add(HavingType havingType, ConditionOperatorType conditionOperator, object conditionValue, ConnectorOperatorType connectorOperator = ConnectorOperatorType.And)
    {
        _groupPredicates.Add(new GroupPredicate<TObject>(havingType, conditionOperator, conditionValue, connectorOperator));
        return this;
    }

    public ICollection<GroupPredicate<TObject>> Return() => _groupPredicates;

    public QueryObjectGrouping<TObject> Validate() => this;
}

public class GroupPredicate<TObject> where TObject : IQueryableObject
{
    public HavingType HavingType
    {
        get;
    }

    public ConditionOperatorType ConditionOperator
    {
        get;
    }

    public object ConditionValue
    {
        get;
    }

    public ConnectorOperatorType ConnectorOperator
    {
        get;
    }

    public GroupPredicate(HavingType havingType, ConditionOperatorType conditionOperator, object conditionValue, ConnectorOperatorType connectorOperator)
    {
        HavingType = havingType;
        ConditionOperator = conditionOperator;
        ConditionValue = conditionValue;
        ConnectorOperator = connectorOperator;
    }
}

