using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Cache;
using Priqraph.Generator.Cache.MemoryCache;
using Priqraph.Generator.Database;
using Priqraph.Helper;
using Priqraph.Query;
using System.Linq.Expressions;

namespace Priqraph.Builder.Cache;

public class MemoryCacheQuery<TObject> : CacheQuery<TObject> where TObject : IQueryableObject
{
    private IObjectInfo<IPropertyInfo> _objectInfo;
    public MemoryCacheQuery(IQueryContext context) : base(context)
    {
        _objectInfo = Context.CacheInfoCollection?.GetLastDatabaseObjectInfo<TObject>() ?? throw new NotFound(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo); //todo
        QueryPartCollection.DatabaseObjectInfo = _objectInfo;
    }

    protected override Task GenerateAddAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, QueryOperationType.Add);
        return Task.CompletedTask;
    }

    protected override Task GenerateUpdateAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, QueryOperationType.Edit);
        return Task.CompletedTask;
    }

    protected override Task GenerateRemoveAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, QueryOperationType.Remove);
        return Task.CompletedTask;
    }

    protected override Task GenerateWhereAsync(QueryObject<TObject> query)
    {
        var expression = query.Filters?.Expression;
        if (expression == null)
            return Task.CompletedTask;

        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new NotSupported(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var whereClause = CacheWhereClauseQueryPart.Create(lambdaExpression);
            QueryPartCollection.WhereClause = whereClause;
        }

        return Task.CompletedTask;
    }

    protected override Task GeneratePagingAsync(QueryObject<TObject> query)
    {
        var expression = query.Paging?.Predicate;
        if (expression == null)
            return Task.CompletedTask;

        if (expression.NodeType != ExpressionType.Lambda)
            throw new NotSupported(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

        var lambdaExpression = (LambdaExpression)expression;
        var body = lambdaExpression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

        var pagingVisitor = new PagingVisitor(Context.CacheInfoCollection, _objectInfo, null);
        QueryPartCollection.Paging = pagingVisitor.Generate(body);

        return Task.CompletedTask;
    }

    protected override Task GenerateOrderByAsync(QueryObject<TObject> query)
    {
        var sortExpression = query.Sorts?.ToList();
        if (sortExpression != null)
        {
            var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);

            var sortingGenerator = new SortingVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var columnSortPredicates = new List<CacheSortPredicate>();
            sortExpression.ToList().ForEach(sortPredicate =>
            {
                Expression? expression = null;
                if (sortPredicate.Expression != null)
                {
                    expression = sortPredicate.Expression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
                }

                if (expression == null)
                    throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                columnSortPredicates.Add(new CacheSortPredicate(sortPredicate.Expression, sortPredicate.DirectionType));
            });
            CacheOrdersByClauseQueryPart.Create(columnSortPredicates.ToArray());
        }

        return Task.CompletedTask;
    }

    private void GenerateRecordCommand(ObjectCommand<TObject> command, CommandVisitor commandSqlVisitor, QueryOperationType operationType)
    {
        var commandQueries = new List<CacheCommandQueryPart>();
        switch (command.CommandValueType)
        {
            case CommandValueType.Record:
                GenerateRecordCommand(command, commandSqlVisitor, commandQueries, operationType);

                break;
            case CommandValueType.Bulk:

                break;
            default:
                throw new NotSupported(ExceptionCode.ApiQueryBuilder); //Too
        }
        commandSqlVisitor.AddOption("returnRecord", command.ReturnType);
        var commandObject = CacheCommandQueryPart.Merge(operationType, commandQueries.ToArray());
        QueryPartCollection.Command = commandObject;
    }

    private static void GenerateRecordCommand(ObjectCommand<TObject> command, CommandVisitor commandSqlVisitor, ICollection<CacheCommandQueryPart> commandQueries, QueryOperationType operationType)
    {
        if (command.ObjectPredicate == null && command.ObjectsPredicate == null) throw new NotFound("as"); //todo

        if (command.ObjectPredicate != null)
        {
            foreach (var objectPredicate in command.ObjectPredicate)
            {
                if (objectPredicate.NodeType != ExpressionType.Lambda)
                    throw new NotSupported(typeof(TObject).Name, objectPredicate.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

                if (objectPredicate is LambdaExpression expression)
                {
                    commandSqlVisitor.AddOption("Command", operationType);
                    var queryCommand = commandSqlVisitor.Generate(expression.Body);
                    commandSqlVisitor.RemoveOption("Command");

                    commandQueries.Add(queryCommand);
                }
                else
                    throw new NotSupported("asd"); //todo
            }
        }

        if (command.ObjectsPredicate != null)
        {
            foreach (var objectsPredicate in command.ObjectsPredicate)
            {
                if (objectsPredicate.NodeType != ExpressionType.Lambda)
                    throw new NotSupported(typeof(TObject).Name, objectsPredicate.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

                if (objectsPredicate is LambdaExpression expression)
                {
                    var queryCommand = commandSqlVisitor.Generate(expression.Body);
                    commandQueries.Add(queryCommand);
                }
                else
                    throw new NotSupported("asd"); //todo
            }
        }
    }
}
