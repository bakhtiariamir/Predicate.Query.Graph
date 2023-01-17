using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Query;

public class QueryObjectCommand<TObject> : IQueryObjectPart<QueryObjectCommand<TObject>, ObjectCommand<TObject>> where TObject : IQueryableObject
{
    private ObjectCommand<TObject> _objectCommand;

    private QueryObjectCommand(CommandValueType commandValueType, QueryPartType commandPartType) => _objectCommand = new ObjectCommand<TObject>(commandPartType, commandValueType);

    public static QueryObjectCommand<TObject> InitInsert(CommandValueType commandValueType) => new(commandValueType, QueryPartType.Insert);

    public static QueryObjectCommand<TObject> InitUpdate(CommandValueType commandValueType) => new(commandValueType, QueryPartType.Update);

    public static QueryObjectCommand<TObject> InitDelete(CommandValueType commandValueType) => new(commandValueType, QueryPartType.Delete);

    public static QueryObjectCommand<TObject> InitMerge(CommandValueType commandValueType) => new(commandValueType, QueryPartType.Merge);

    public QueryObjectCommand<TObject> Add(Expression<Func<TObject, TObject>> expression)
    {
        _objectCommand.AddObjectPredicate(expression);
        return this;
    }

    public QueryObjectCommand<TObject> AddRange(Expression<Func<TObject, IEnumerable<TObject>>> expression)
    {
        _objectCommand.AddObjectsPredicate(expression);
        return this;
    }

    public QueryObjectCommand<TObject> SetFilter(QueryObjectFiltering<TObject> filter)
    {
        _objectCommand.SetObjectFiltering(filter);
        return this;
    }

    public QueryObjectCommand<TObject> Validate() => this;

    public ObjectCommand<TObject> Return() => _objectCommand;
}

public class ObjectCommand<TObject> where TObject : IQueryableObject
{
    public CommandValueType CommandValueType
    {
        get;
    }

    public QueryPartType CommandPartType
    {
        get;
    }

    public ICollection<Expression<Func<TObject, TObject>>>? ObjectPredicate
    {
        get;
        private set;
    }

    public ICollection<Expression<Func<TObject, IEnumerable<TObject>>>>? ObjectsPredicate
    {
        get;
        private set;
    }

    public QueryObjectFiltering<TObject>? Filter
    {
        get;
        private set;
    }

    public ObjectCommand(QueryPartType commandPartType, CommandValueType commandValueType)
    {
        CommandValueType = commandValueType;
        CommandPartType = commandPartType;
    }

    public void SetObjectPredicate(ICollection<Expression<Func<TObject, TObject>>> predicate) => ObjectPredicate = predicate;

    public void AddObjectPredicate(Expression<Func<TObject, TObject>> predicate)
    {
        if (ObjectPredicate == null) SetObjectPredicate(new[] {predicate});
        else ObjectPredicate.Add(predicate);
    }

    public void AddObjectsPredicate(Expression<Func<TObject, IEnumerable<TObject>>> predicates)
    {
        if (ObjectsPredicate == null) SetObjectsPredicate(new[] {predicates});
        else ObjectsPredicate.Add(predicates);
    }

    public void SetObjectsPredicate(ICollection<Expression<Func<TObject, IEnumerable<TObject>>>> predicates) => ObjectsPredicate = predicates;

    public void SetObjectFiltering(QueryObjectFiltering<TObject> filter) => Filter = filter;
}
