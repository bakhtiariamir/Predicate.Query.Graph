using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Database;
using Parsis.Predicate.Sdk.Generator.Database.SqlServer;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Builder.Database;

public class SqlServerQuery<TObject> : DatabaseQuery<TObject> where TObject : IQueryableObject
{
    private IDatabaseObjectInfo _objectInfo;
    public SqlServerQuery(IQueryContext context) : base(context)
    {
        _objectInfo = Context.CacheInfoCollection?.GetLastDatabaseObjectInfo<TObject>() ?? throw new NotFound(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo);
        QueryPartCollection.DatabaseObjectInfo = _objectInfo;
    }

    protected override async Task GenerateInsertAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var commandSqlVisitor = new SqlServerCommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Add);

        if (QueryPartCollection.Command != null && command.ReturnType == ReturnType.Record && QueryPartCollection.Command.CommandParts.ContainsKey("result"))
        {
            if (query.Columns == null)
                throw new ArgumentNullException($"Columns for insert query in return record mode for {typeof(TObject).Name} can not be null.");

            var selectQuery = QueryObjectBuilder<TObject>.Init(QueryObject<TObject>.Init(QueryOperationType.GetData)).SetSelecting(query.Columns).SetFilterPredicate(QueryObjectFiltering<TObject>.Init(ReturnType.Record).Return()).Generate();
            var sqlQuery = await Build(selectQuery);
            QueryPartCollection.ResultQuery = sqlQuery;
        }
    }

    protected override async Task GenerateUpdateAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var commandSqlVisitor = new SqlServerCommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Edit);
        if (QueryPartCollection.Command != null && command.ReturnType == ReturnType.Record && QueryPartCollection.Command.CommandParts.ContainsKey("result"))
        {
            if (query.Columns == null)
                throw new ArgumentNullException($"Columns for update query in return record mode for {typeof(TObject).Name} can not be null.");


            var selectQuery = QueryObjectBuilder<TObject>.Init(QueryObject<TObject>.Init(QueryOperationType.GetData)).SetSelecting(query.Columns).SetFilterPredicate(QueryObjectFiltering<TObject>.Init(ReturnType.Record).Return()).Generate();
            var sqlQuery = await Build(selectQuery);
            QueryPartCollection.ResultQuery = sqlQuery;
        }
    }

    protected override Task GenerateDeleteAsync(QueryObject<TObject> query)
    {
        var command = query.Command ?? throw new NotSupported("a");
        var commandSqlVisitor = new SqlServerCommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Remove);
        return Task.CompletedTask;
    }

    protected override Task GenerateColumnAsync(QueryObject<TObject> query, bool getCount = false)
    {
        if (!getCount)
        {
            var fieldsExpression = query.Columns?.ToList();
            ParameterExpression? parameterExpression = null;
            if (fieldsExpression[0]?.Expression != null)
            {
                parameterExpression = fieldsExpression[0].Expression?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
            }
            else if (fieldsExpression[0]?.Expressions != null)
            {
                parameterExpression = fieldsExpression[0].Expressions?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
            }

            var selectingGenerator = new SqlServerSelectingVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var columns = new List<DatabaseColumnsClauseQueryPart>();
            fieldsExpression.ToList().ForEach(field =>
            {
                Expression? expression = null;
                if (field.Expression != null)
                    expression = field.Expression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                if (field.Expressions != null)
                    expression = field.Expressions.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                if (expression == null)
                    throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                var property = selectingGenerator.Generate(expression);
                columns.Add(property);
            });

            var queryColumns = DatabaseColumnsClauseQueryPart.Merged(columns.Select(item => item));
            QueryPartCollection.Columns = queryColumns;
            JoinColumns.AddRange(queryColumns.Parameter);
        }
        else
        {
            QueryPartCollection.Columns = DatabaseColumnsClauseQueryPart.CreateCount();
        }

        return Task.CompletedTask;
    }

    protected override Task GenerateWhereAsync(QueryObject<TObject> query)
    {
        if (query.Filters?.ReturnType != ReturnType.None)
        {
            var key = _objectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException("Primary key can not be null.");
            var clause = new WhereClause(key, null, ConditionOperatorType.Equal);
            switch (query.Filters?.ReturnType)
            {
                case ReturnType.Record:
                    var whereClause = DatabaseWhereClauseQueryPart.Create(clause);
                    whereClause.SetText(ReturnType.Record, clause);
                    QueryPartCollection.WhereClause = whereClause;
                    break;
            }
        }

        var expression = query.Filters?.Expression;
        if (expression == null)
            return Task.CompletedTask;

        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new NotSupported(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var body = lambdaExpression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            var parameterExpression = lambdaExpression.Parameters[0];
            var whereGenerator = new SqlServerFilteringVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var whereClause = whereGenerator.Generate(body);
            whereClause.ReduceParameter();
            whereClause.SetText();
            QueryPartCollection.WhereClause = whereClause;
            var joinColumns = DatabaseWhereClauseQueryPart.GetColumnProperties(whereClause.Parameter);
            JoinColumns.AddRange(joinColumns);
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

        var pagingVisitor = new SqlServerPagingVisitor(Context.CacheInfoCollection, _objectInfo, null);
        QueryPartCollection.Paging = pagingVisitor.Generate(body);

        return Task.CompletedTask;
    }

    protected override Task GenerateOrderByAsync(QueryObject<TObject> query)
    {
        var sortExpression = query.Sorts?.ToList();
        if (sortExpression != null)
        {
            var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
            var sortingGenerator = new SqlServerSortingVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            sortExpression.ToList().ForEach(sortPredicate =>
            {
                Expression? expression = null;
                if (sortPredicate.Expression != null)
                {
                    expression = sortPredicate.Expression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
                }

                if (expression == null)
                    throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                var orderByProperty = sortingGenerator.Generate(expression);
                QueryPartCollection.OrderByClause = QueryPartCollection.OrderByClause == null ? orderByProperty : DatabaseOrdersByClauseQueryPart.Merged(new[] { QueryPartCollection.OrderByClause, orderByProperty });
            });
        }

        return Task.CompletedTask;
    }

    protected override Task GenerateJoinAsync()
    {
        var joins = new List<Tuple<IColumnPropertyInfo, int>>();

        var joinGenerator = new SqlServerJoiningVisitor(Context.CacheInfoCollection, _objectInfo, null);

        Action<ICollection<IColumnPropertyInfo>, int>? getJoins = null;
        getJoins = (parameters, level) =>
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Parent is null || parameter.Parent.Name == _objectInfo.DataSet)
                    continue;

                if (joins.Any(item => item.Item1.Equals(parameter.Parent)))
                    continue;

                joins.Add(new Tuple<IColumnPropertyInfo, int>(parameter.Parent, level));
                getJoins?.Invoke(new[] {parameter.Parent}, ++level);
            }
        };

        getJoins(JoinColumns.Where(item => item.Parent is not null && item.Parent.Name != _objectInfo.DataSet).DistinctBy(item => new {item.Schema, item.DataSet, item.ColumnName, item.Name}).ToList(), 0);
        var queryObjectJoining = QueryObjectJoining.Init();

        joins.OrderBy(item => item.Item2).Select(item => item).GroupBy(item => new {item.Item1.Schema, item.Item1.DataSet, item.Item1.ColumnName, item.Item1.Name}).ToList().ForEach(item =>
        {
            var index = 0;
            foreach (var joinPredicate in item.Select(item => item))
            {
                var join = joinPredicate.Item1;
                var order = joinPredicate.Item2;
                if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Parent.Type, out var propertyObjectInfo))
                    throw new NotFound(@join.DataSet, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (propertyObjectInfo == null)
                    throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Type, out var relatedObjectInfo))
                    throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (relatedObjectInfo == null)
                    throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                var joinColumnPropertyInfo = relatedObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key)?.Clone() ?? throw new NotFound(@join.Name, "Primary_Key", ExceptionCode.DatabaseQueryJoiningGenerator);

                joinColumnPropertyInfo.Parent = @join;

                var indexer = index > 0 ? $"_{index}" : "";
                var joinType = @join.Required ? JoinType.Inner : JoinType.Left;
                var expression = joinColumnPropertyInfo.GenerateJoinExpression(propertyObjectInfo.ObjectType, joinType, indexer);
                queryObjectJoining.Add(expression, joinType, order);

                index++;
            }
        });

        var joinPredicates = queryObjectJoining.Validate().Return().OrderBy(item => item.Order);
        var databaseJoinClauses = new List<DatabaseJoinsClauseQueryPart>();
        foreach (var join in joinPredicates)
        {
            joinGenerator.AddOption("JoinType", join.Type);
            databaseJoinClauses.Add(joinGenerator.Generate(join.PropertyExpression));
            joinGenerator.RemoveOption("JoinType");
        }

        QueryPartCollection.JoinClause = DatabaseJoinsClauseQueryPart.Merged(databaseJoinClauses);

        return Task.CompletedTask;
    }

    protected override Task GenerateFunctionByClause()
    {
        var columnQueryPart = QueryPartCollection.Columns ?? throw new NotFound(ExceptionCode.DatabaseQueryGroupByGenerator);
        var whereQueryPart = QueryPartCollection.WhereClause;

        var grouping = false;
        ICollection<WhereClause>? havingClauses = null;
        if (whereQueryPart != null)
        {
            havingClauses = DatabaseWhereClauseQueryPart.GeHavingClause(whereQueryPart.Parameter);
            grouping = havingClauses.Count > 0;
        }

        if (!grouping) return Task.CompletedTask;
        var groupingColumns = columnQueryPart.Parameter.Where(item => item.AggregateFunctionType == AggregateFunctionType.None) ?? throw new NotFound(ExceptionCode.DatabaseQueryGroupByGenerator);
        var groupClauses = DatabaseGroupByClauseQueryPart.Create(new GroupingPredicate(groupingColumns, havingClauses));

        groupClauses.GroupingText();
        groupClauses.HavingText();

        QueryPartCollection.GroupByClause = groupClauses;

        return Task.CompletedTask;
    }

    private void GenerateRecordCommand(ObjectCommand<TObject> command, SqlServerCommandVisitor commandSqlVisitor, QueryOperationType operationType)
    {
        var commandQueries = new List<DatabaseCommandQueryPart>();
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
        var commandObject = DatabaseCommandQueryPart.Merge(operationType, command.ReturnType, commandQueries.ToArray());
        QueryPartCollection.Command = commandObject;
    }

    private static void GenerateRecordCommand(ObjectCommand<TObject> command, SqlServerCommandVisitor commandSqlVisitor, ICollection<DatabaseCommandQueryPart> commandQueries, QueryOperationType operationType)
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
