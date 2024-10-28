using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Cache;
using Priqraph.Generator.Cache.MemoryCache;
using Priqraph.Helper;
using Priqraph.Query.Predicates;
using System.Linq.Expressions;

namespace Priqraph.Builder.Cache;

internal class MemoryCacheQueryObject<TObject> : CacheQueryObject<TObject> where TObject : IQueryableObject
{
    private IObjectInfo<IPropertyInfo> _objectInfo;
    public MemoryCacheQueryObject(IQueryContext context) : base(context)
    {
        _objectInfo = Context.CacheInfoCollection?.LastDatabaseObjectInfo<TObject>() ?? throw new NotFoundException(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo); //todo
        QueryResult.DatabaseObjectInfo = _objectInfo;
    }

    protected override Task GenerateAdd(IQuery<TObject> query)
    {
        var command = query.CommandPredicates ?? throw new NotSupportedOperationException("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, DatabaseQueryOperationType.Add);
        return Task.CompletedTask;
    }

    protected override Task GenerateUpdate(IQuery<TObject> query)
    {
        var command = query.CommandPredicates ?? throw new NotSupportedOperationException("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, DatabaseQueryOperationType.Edit);
        return Task.CompletedTask;
    }

    protected override Task GenerateRemove(IQuery<TObject> query)
    {
        var command = query.CommandPredicates ?? throw new NotSupportedOperationException("a");
        var cacheCommandVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, cacheCommandVisitor, DatabaseQueryOperationType.Remove);
        return Task.CompletedTask;
    }

    protected override Task GenerateWhere(IQuery<TObject> query)
    {
        var expression = query.FilterPredicates?.Expression;
        if (expression == null)
            return Task.CompletedTask;

        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new NotSupportedOperationException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var whereClause = CacheWhereClauseQueryPart.Create(lambdaExpression);
            QueryResult.WhereClause = whereClause;
        }

        return Task.CompletedTask;
    }

    protected override Task GeneratePaging(IQuery<TObject> query)
    {
        var expression = query.PagePredicate?.Predicate;
        if (expression == null)
            return Task.CompletedTask;

        if (expression.NodeType != ExpressionType.Lambda)
            throw new NotSupportedOperationException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

        var lambdaExpression = (LambdaExpression)expression;
        var body = lambdaExpression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

        var pagingVisitor = new PagingVisitor(Context.CacheInfoCollection, _objectInfo, null);
        QueryResult.Paging = pagingVisitor.Generate(body);

        return Task.CompletedTask;
    }

    protected override Task GenerateOrderBy(IQuery<TObject> query)
    {
        var sortExpression = query.SortPredicates?.ToList();
        if (sortExpression != null)
        {
            var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);

            var sortingGenerator = new SortingVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var columnSortPredicates = new List<CacheSortPredicate>();
            sortExpression.ToList().ForEach(sortPredicate =>
            {
                Expression? expression = null;
                if (sortPredicate.Expression != null)
                {
                    expression = sortPredicate.Expression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
                }

                if (expression == null)
                    throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                columnSortPredicates.Add(new CacheSortPredicate(sortPredicate.Expression, sortPredicate.DirectionType));
            });
            CacheOrdersByClauseQueryPart.Create(columnSortPredicates.ToArray());
        }

        return Task.CompletedTask;
    }

    private void GenerateRecordCommand(CommandPredicate<TObject> commandPredicate, CommandVisitor commandSqlVisitor, DatabaseQueryOperationType operationType)
    {
        var commandQueries = new List<CacheCommandQueryPart>();
        switch (commandPredicate.CommandValueType)
        {
            case CommandValueType.Record:
                GenerateRecordCommand(commandPredicate, commandSqlVisitor, commandQueries, operationType);

                break;
            case CommandValueType.Bulk:

                break;
            default:
                throw new NotSupportedOperationException(ExceptionCode.ApiQueryBuilder); //Too
        }
        commandSqlVisitor.AddOption("returnRecord", commandPredicate.ReturnType);
        var commandObject = CacheCommandQueryPart.Merge(operationType, commandQueries.ToArray());
        QueryResult.Command = commandObject;
    }

    private static void GenerateRecordCommand(CommandPredicate<TObject> commandPredicate, CommandVisitor commandSqlVisitor, ICollection<CacheCommandQueryPart> commandQueries, DatabaseQueryOperationType operationType)
    {
        if (commandPredicate.ObjectPredicate == null && commandPredicate.ObjectsPredicate == null) throw new NotFoundException("as"); //todo

        if (commandPredicate.ObjectPredicate != null)
        {
            foreach (var objectPredicate in commandPredicate.ObjectPredicate)
            {
                if (objectPredicate.NodeType != ExpressionType.Lambda)
                    throw new NotSupportedOperationException(typeof(TObject).Name, objectPredicate.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

                if (objectPredicate is LambdaExpression expression)
                {
                    commandSqlVisitor.AddOption("Command", operationType);
                    var queryCommand = commandSqlVisitor.Generate(expression.Body);
                    commandSqlVisitor.RemoveOption("Command");

                    commandQueries.Add(queryCommand);
                }
                else
                    throw new NotSupportedOperationException("asd"); //todo
            }
        }

        if (commandPredicate.ObjectsPredicate != null)
        {
            foreach (var objectsPredicate in commandPredicate.ObjectsPredicate)
            {
                if (objectsPredicate.NodeType != ExpressionType.Lambda)
                    throw new NotSupportedOperationException(typeof(TObject).Name, objectsPredicate.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

                if (objectsPredicate is LambdaExpression expression)
                {
                    var queryCommand = commandSqlVisitor.Generate(expression.Body);
                    commandQueries.Add(queryCommand);
                }
                else
                    throw new NotSupportedOperationException("asd"); //todo
            }
        }
    }
}
