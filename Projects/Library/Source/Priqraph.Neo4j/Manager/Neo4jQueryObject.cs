using System.Linq.Expressions;
using Priqraph.Builder;
using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator;
using Priqraph.Helper;
using Priqraph.Info;
using Priqraph.Neo4j.Builder;
using Priqraph.Neo4j.Generator;
using Priqraph.Neo4j.Generator.Visitors;
using Priqraph.Query;
using Priqraph.Query.Builders;
using Priqraph.Query.Predicates;
using Priqraph.Setup;

namespace Priqraph.Neo4j.Manager;

internal class Neo4jQueryObject<TObject, TObjectQuery> : Neo4jQuery<TObject> , INeo4jQueryObject<TObject>
    where TObject : IQueryableObject
    where TObjectQuery : INeo4jQuery<TObject, Neo4jQueryOperationType>
{
    private readonly IDatabaseObjectInfo _objectInfo;
    public override INeo4jQuery<TObject, Neo4jQueryOperationType> Query { get; }
    public Neo4jQueryResult Build(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        throw new NotImplementedException();
    }

    public Neo4jQueryObject(ICacheInfoCollection cacheInfoCollection) : base(cacheInfoCollection)
    {
        _objectInfo = cacheInfoCollection?.LastDatabaseObjectInfo<TObject>() ??
                      throw new NotFoundException(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo);
        
        QueryResult.DatabaseObjectInfo = _objectInfo;
    }

    protected override void GenerateInsert(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var command = query.CommandPredicates ?? throw new NotSupportedOperationException("a");
        var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, default);
        GenerateRecordCommand(command, commandSqlVisitor, Neo4jQueryOperationType.InsertNode);

        if (QueryResult.CommandFragment != null && command.ReturnType == ReturnType.Record && QueryResult.CommandFragment.CommandParts.ContainsKey("result"))
        {
            if (query.ColumnPredicates == null)
                throw new ArgumentNullException($"Columns for insert query in return record mode for {typeof(TObject).Name} can not be null.");

            var builder = new Neo4jQueryBuilder<TObject>();
            builder.Init(Neo4jQueryOperationType.DeleteLeaf, QueryProvider.SqlServer, query, query.ObjectTypeStructures);
            var selectQuery = builder.SetColumns(query.ColumnPredicates).SetFilter(FilterPredicateBuilder<TObject>.Init(ReturnType.Record).Return()).Generate();
            var sqlQuery = Build(selectQuery);
            QueryResult.ResultQuery = sqlQuery;
        }
    }

    protected override void GenerateUpdate(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var command = query.CommandPredicates ??
                      throw new ArgumentNullException(nameof(query.CommandPredicates), "Command Predicate cannot null");
        
        var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, commandSqlVisitor, Neo4jQueryOperationType.FindNode);
        if (QueryResult.CommandFragment != null && command.ReturnType == ReturnType.Record && QueryResult.CommandFragment.CommandParts.ContainsKey("result"))
        {
            if (query.ColumnPredicates == null)
                throw new ArgumentNullException($"Columns for update query in return record mode for {typeof(TObject).Name} can not be null.");

            var builder = new Neo4jQueryBuilder<TObject>();
            builder.Init(Neo4jQueryOperationType.FindNode, QueryProvider.Neo4J, query, query.ObjectTypeStructures);
            var selectQuery = builder.SetColumns(query.ColumnPredicates).SetFilter(FilterPredicateBuilder<TObject>.Init(ReturnType.Record).Return()).Generate();
            var sqlQuery = Build(selectQuery);
            QueryResult.ResultQuery = sqlQuery;
        }
    }

    protected override void GenerateDelete(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var command = query.CommandPredicates ??
                      throw new ArgumentNullException(nameof(query.CommandPredicates), "Command Predicate cannot null");
        
        var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
        GenerateRecordCommand(command, commandSqlVisitor, Neo4jQueryOperationType.InsertLeaf);

    }
    
    protected override void GenerateColumn(INeo4jQuery<TObject, Neo4jQueryOperationType> query, bool getCount = false)
    {
        if (!getCount)
        {
            var fieldsExpression = query.ColumnPredicates?.ToList();
            ParameterExpression? parameterExpression = null;
            if (fieldsExpression?[0].Expression != null)
            {
                parameterExpression = fieldsExpression[0].Expression?.Parameters[0] ??
                                      throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter",
                                          ExceptionCode.DatabaseQueryFilteringGenerator);
            }
            else if (fieldsExpression?[0].Expressions != null)
            {
                parameterExpression = fieldsExpression[0].Expressions?.Parameters[0] ??
                                      throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter",
                                          ExceptionCode.DatabaseQueryFilteringGenerator);
            }

            var selectingGenerator = new ColumnVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var columns = new List<Neo4jColumnQueryFragment>();
            fieldsExpression?.ToList().ForEach(field =>
            {
                Expression? expression = null;
                if (field.Expression != null)
                    expression = field.Expression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body",
                        ExceptionCode.DatabaseQueryFilteringGenerator);

                if (field.Expressions != null)
                    expression = field.Expressions.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body",
                        ExceptionCode.DatabaseQueryFilteringGenerator);

                if (expression == null)
                    throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                var property = selectingGenerator.Generate(expression);
                columns.Add(property);
            });

            var queryColumns = Neo4jColumnQueryFragment.Merged(columns.Select(item => item));
            QueryResult.ColumnFragment = queryColumns;
            JoinColumns.AddRange(queryColumns.Parameter ?? new List<IColumnPropertyInfo>());
        }
        else
        {
            QueryResult.ColumnFragment = Neo4jColumnQueryFragment.CreateCount();
        }


    }

    protected override void GenerateWhere(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        if (query.FilterPredicates?.ReturnType != ReturnType.None)
        {
            var key = _objectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException(nameof(PropertyInfo.Key), "Primary key can not be null.");
            var clause = new FilterProperty(key, null, ConditionOperatorType.Equal);
            switch (query.FilterPredicates?.ReturnType)
            {
                case ReturnType.Record:
                    var whereClause = Neo4jFilterQueryFragment.Create(clause);
                    whereClause.SetText(ReturnType.Record, clause);
                    QueryResult.FilterFragment = whereClause;
                    break;
                case ReturnType.None:
                    break;
                case ReturnType.Key:
                    break;
                case ReturnType.RowAffected:
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var expression = query.FilterPredicates?.Expression;
        if (expression == null) return;
        
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new NotSupportedOperationException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var body = lambdaExpression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            var parameterExpression = lambdaExpression.Parameters[0];
            var whereGenerator = new FilterVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
            var whereClause = whereGenerator.Generate(body);
            whereClause.SetQuerySetting(Context.CacheInfoCollection.QuerySetting);
            whereClause.ReduceParameter();
            whereClause.SetText();
            QueryResult.FilterFragment = whereClause;
            if (whereClause.Parameter != null)
            {
                var joinColumns = Neo4jFilterQueryFragment.GetColumnProperties(whereClause.Parameter);
                JoinColumns.AddRange(joinColumns);
            }
        }


    }

    protected override void GeneratePaging(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var expression = query.PagePredicate?.Predicate;
        if (expression == null) return;
        
        if (expression.NodeType != ExpressionType.Lambda)
            throw new NotSupportedOperationException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

        var lambdaExpression = (LambdaExpression)expression;
        var body = lambdaExpression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

        var pagingVisitor = new PageVisitor(Context.CacheInfoCollection, _objectInfo, default);
        QueryResult.PageFragment = pagingVisitor.Generate(body);
    }
    
    protected override void GenerateOrderBy(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var sortExpression = query.SortPredicates?.ToList();
        if (sortExpression == null) return;
        
        var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
        var sortingGenerator = new SortVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
        sortExpression.ToList().ForEach(sortPredicate =>
        {
            Expression? expression = default;
            if (sortPredicate.Expression != null)
            {
                expression = sortPredicate.Expression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            }

            if (expression == null)
                throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            var orderByProperty = sortingGenerator.Generate(expression);
            QueryResult.SortFragment = QueryResult.SortFragment == null ? orderByProperty : Neo4jSortQueryFragment.Merged(new[] { (Neo4jSortQueryFragment)QueryResult.SortFragment, orderByProperty });
        });
    }

    protected override void GenerateJoin(INeo4jQuery<TObject, Neo4jQueryOperationType> query)
    {
        var joins = new List<Tuple<IColumnPropertyInfo, int>>();

        var joinGenerator = new JoinVisitor(Context.CacheInfoCollection, _objectInfo, default);

        Action<ICollection<IColumnPropertyInfo>, int>? getJoins = null;
        getJoins = (parameters, level) =>
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Parent is null || parameter.Parent.Name == _objectInfo.DataSet)
                    continue;

                if (joins.Any(item => item.Item1.Equals(parameter.Parent)))
                    continue;

                if (parameter.FieldType == DatabaseFieldType.Related)
                    continue;

                joins.Add(new Tuple<IColumnPropertyInfo, int>(parameter.Parent, level));
                getJoins?.Invoke(new[] { parameter.Parent }, ++level);
            }
        };

        getJoins(JoinColumns.Where(item => item.Parent is not null && item.Parent.Name != _objectInfo.DataSet).DistinctBy(item => new { item.Schema, item.DataSet, item.ColumnName, item.Name }).ToList(), 0);
        var queryObjectJoining = JoinPredicateBuilder.Init();

        joins.OrderBy(item => item.Item2).Select(item => item).GroupBy(item => new { item.Item1.Schema, item.Item1.DataSet, item.Item1.ColumnName, item.Item1.Name }).ToList().ForEach(item =>
        {
            var index = 0;
            foreach (var (join, order) in item.Select(joinItem => joinItem))
            {
                if (join.Parent is null)
                    continue;
                
                if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Parent.Type, out var propertyObjectInfo))
                    throw new NotFoundException(join.DataSet, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (propertyObjectInfo == null)
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Type, out var relatedObjectInfo))
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                if (relatedObjectInfo == null)
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                var joinColumnPropertyInfo = relatedObjectInfo.PropertyInfos.FirstOrDefault(relatedObject => relatedObject.Key)?.Clone() ?? throw new NotFoundException(join.Name, "Primary_Key", ExceptionCode.DatabaseQueryJoiningGenerator);

                joinColumnPropertyInfo.Parent = join;

                var indexer = index > 0 ? $"_{index}" : "";
                var joinType = join.Required ? JoinType.Inner : JoinType.Left;
                var expression = joinColumnPropertyInfo.GenerateJoinExpression(propertyObjectInfo.ObjectType, joinType, query.ObjectTypeStructures.ToArray(), indexer);
                queryObjectJoining.Add(expression, joinType, order);

                index++;
            }
        });

        var joinPredicates = queryObjectJoining.Validate().Return().OrderBy(item => item.Order);
        var databaseJoinClauses = new List<Neo4JJoinQueryFragment>();
        foreach (var join in joinPredicates)
        {
            joinGenerator.AddOption("JoinType", join.Type);
            databaseJoinClauses.Add(joinGenerator.Generate(join.PropertyExpression));
            joinGenerator.RemoveOption("JoinType");
        }

        QueryResult.JoinFragment = Neo4JJoinQueryFragment.Merged(databaseJoinClauses);


    }

    protected override void GenerateFunctionByClause()
    {
        var columnQueryPart = QueryResult.ColumnFragment ?? throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);
        var whereQueryPart = QueryResult.FilterFragment;

        var grouping = false;
        ICollection<FilterProperty>? havingClauses = null;
        if (whereQueryPart is {Parameter: not null})
        {
            havingClauses = Neo4jFilterQueryFragment.GetHavingClause(whereQueryPart.Parameter);
            grouping = havingClauses.Count > 0;
        }

        if (grouping)
        {
            IEnumerable<IColumnPropertyInfo> groupingColumns = columnQueryPart.Parameter?.Where(item => item.AggregateFunctionType == AggregateFunctionType.None) ?? throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);

            var groupClauses = Neo4jGroupByQueryFragment.Create(new GroupByProperty(groupingColumns, havingClauses));

            groupClauses.GroupingText();
            groupClauses.HavingText();

            QueryResult.GroupByFragment = groupClauses;

        }
    }

    private void GenerateRecordCommand(CommandPredicate<TObject> commandPredicate, CommandVisitor commandSqlVisitor, Neo4jQueryOperationType operationType)
    {
        var commandQueries = new List<Neo4JCommandQueryFragment>();
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
        var commandObject = Neo4JCommandQueryFragment.Merge(operationType, commandPredicate.ReturnType, commandQueries.ToArray());
        QueryResult.CommandFragment = commandObject;
    }

    private static void GenerateRecordCommand(CommandPredicate<TObject> commandPredicate, CommandVisitor commandSqlVisitor, ICollection<Neo4JCommandQueryFragment> commandQueries, Neo4jQueryOperationType operationType)
    {
        if (commandPredicate.ObjectPredicate == null && commandPredicate.ObjectsPredicate == null)
            throw new ArgumentNullException(nameof(commandPredicate.ObjectPredicate));

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
                    throw new NotSupportedOperationException(ExceptionCode.SqlServerQueryCreator);
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
