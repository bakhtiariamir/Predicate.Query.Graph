using Dynamitey;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Query.Builders;

public class CommandPredicateBuilder<TObject> : IQueryObjectPart<CommandPredicateBuilder<TObject>, CommandPredicate<TObject>> where TObject : IQueryableObject
{
    private CommandPredicate<TObject> _commandPredicate;

    private CommandPredicateBuilder(CommandValueType commandValueType, QueryPartType commandPartType, ReturnType returnType = ReturnType.None) => _commandPredicate = new CommandPredicate<TObject>(commandPartType, commandValueType, returnType);

    public static CommandPredicateBuilder<TObject> InitInsert(CommandValueType commandValueType, ReturnType returnType = ReturnType.None) => new(commandValueType, QueryPartType.Insert, returnType);

    public static CommandPredicateBuilder<TObject> InitUpdate(CommandValueType commandValueType, ReturnType returnType = ReturnType.None) => new(commandValueType, QueryPartType.Update, returnType);

    public static CommandPredicateBuilder<TObject> InitDelete(CommandValueType commandValueType) => new(commandValueType, QueryPartType.Delete);

    public static CommandPredicateBuilder<TObject> InitMerge(CommandValueType commandValueType, ReturnType returnType = ReturnType.None) => new(commandValueType, QueryPartType.Merge, returnType);

    public CommandPredicateBuilder<TObject> Add(Expression<Func<TObject, TObject>> expression)
    {
        _commandPredicate.AddObjectPredicate(expression);
        return this;
    }

    public CommandPredicateBuilder<TObject> AddRange(Expression<Func<TObject, IEnumerable<TObject>>> expression)
    {
        _commandPredicate.AddObjectsPredicate(expression);
        return this;
    }

    public CommandPredicateBuilder<TObject> SetFilter(FilterPredicateBuilder<TObject> filter)
    {
        _commandPredicate.SetObjectFiltering(filter);
        return this;
    }

    public CommandPredicateBuilder<TObject> Validate() => this;

    public CommandPredicate<TObject> Return() => _commandPredicate;

    public Dictionary<string, string> GetQueryOptions() => new();
}

