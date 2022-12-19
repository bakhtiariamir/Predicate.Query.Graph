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

    public SqlServerQuery(IQueryContext context, DatabaseQueryOperationType queryType) : base(context, queryType)
    {
        _objectInfo = Context.DatabaseCacheInfoCollection?.GetLastObjectInfo<TObject>() ?? throw new NotFoundException(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo);
        QueryPartCollection.DatabaseObjectInfo = _objectInfo;
    }



    protected override Task GenerateColumn(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        var fieldsExpression = query.Columns?.ToList();
        var parameterExpression = fieldsExpression[0].Expression?.Parameters[0] ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
        var selectingGenerator = new SqlServerSelectingGenerator(Context.DatabaseCacheInfoCollection, _objectInfo, parameterExpression);
        var columns = new List<DatabaseColumnsClauseQueryPart>();
        fieldsExpression.ToList().ForEach(field =>
        {
            Expression? expression = null;
            if (field.Expression != null)
                expression = field.Expression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            if (field.Expressions != null)
                expression = field.Expressions.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            if (expression == null)
                throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            var property = selectingGenerator.Generate(expression);
            columns.Add(property);
        });

        var queryColumns = DatabaseColumnsClauseQueryPart.CreateMerged(columns.Select(item => item));
        QueryPartCollection.Columns = queryColumns;
        JoinColumns.AddRange(queryColumns.Parameter);

        return Task.CompletedTask;
    }

    protected override Task GenerateWhereClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        var expression = query.Filters?.Expression;
        if (expression != null)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new Exception.NotSupportedException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var body = lambdaExpression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            var parameterExpression = lambdaExpression.Parameters[0];
            var whereGenerator = new SqlServerFilteringGenerator(Context.DatabaseCacheInfoCollection, _objectInfo, parameterExpression);
            var whereClause = whereGenerator.Generate(body);
            whereClause.ReduceParameter();
            whereClause.SetText();
            QueryPartCollection.WhereClause = whereClause;
            var joinColumns = DatabaseWhereClauseQueryPart.GetColumnProperties(whereClause.Parameter);
            JoinColumns.AddRange(joinColumns);
        }

        return Task.CompletedTask;
    }

    protected override Task GeneratePagingClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        var expression = query.Paging?.Predicate;
        if (expression != null)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new Exception.NotSupportedException(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

            var lambdaExpression = (LambdaExpression)expression;
            var body = lambdaExpression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            var pagingVisitor = new SqlServerPagingVisitor(Context.DatabaseCacheInfoCollection, _objectInfo, null);
            QueryPartCollection.Paging = pagingVisitor.Generate(body);
        }

        return Task.CompletedTask;
    }

    protected override Task GenerateOrderByClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        var sortExpression = query.Sorts?.ToList();
        if (sortExpression != null)
        {
            var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
            var sortingGenerator = new SqlServerSortingGenerator(Context.DatabaseCacheInfoCollection, _objectInfo, parameterExpression);
            sortExpression.GroupBy(item => item.DirectionType).ToList().ForEach(sortPredicate =>
            {
                Expression? expression = null;
                var direction = sortPredicate.Key;

                foreach (var field in sortPredicate)
                {
                    if (field.Expression != null)
                    {
                        expression = field.Expression.Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
                    }

                    if (expression == null)
                        throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

                    var orderByProperty = sortingGenerator.Generate(expression);
                    QueryPartCollection.OrderByClause = QueryPartCollection.OrderByClause == null ? orderByProperty : DatabaseOrdersByClauseQueryPart.CreateMerged(new[] { QueryPartCollection.OrderByClause, orderByProperty });
                }
            });
        }

        return Task.CompletedTask;
    }

    protected override Task GenerateJoinClause()
    {
        var joins = new List<Tuple<IColumnPropertyInfo, int>>();

        var joinGenerator = new SqlServerJoiningGenerator(Context.DatabaseCacheInfoCollection, _objectInfo, null);

        Action<ICollection<IColumnPropertyInfo>, int>? getJoins = null;
        getJoins = (parameters, level) =>
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Parent is not null && !parameter.IsPrimaryKey)
                    if (joins.All(item => !item.Item1.Equals(parameter.Parent)))
                    {
                        joins.Add(new Tuple<IColumnPropertyInfo, int>(parameter.Parent, level));
                        getJoins?.Invoke(new[] { parameter.Parent }, ++level);
                    }
            }
        };

        getJoins(JoinColumns.DistinctBy(item => new { item.Schema, item.DataSet, item.ColumnName, item.Name }).ToList(), 0);
        QueryObjectJoining queryObjectJoining = QueryObjectJoining.Init();
        joins.OrderByDescending(item => item.Item2).Select(item => item).
            GroupBy(item => new { item.Item1.Schema, item.Item1.DataSet, item.Item1.ColumnName, item.Item1.Name }).ToList().ForEach(item =>
            {
                var index = 0;
                foreach (var joinPredicate in item.Select(item => item))
                {
                    var join = joinPredicate.Item1;
                    var order = joinPredicate.Item2;
                    if (!Context.DatabaseCacheInfoCollection.TryGet(join.DataSet, out IDatabaseObjectInfo? propertyObjectInfo))
                        throw new NotFoundException(@join.DataSet, ExceptionCode.DatabaseQueryJoiningGenerator);

                    if (propertyObjectInfo == null)
                        throw new NotFoundException(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                    if (!Context.DatabaseCacheInfoCollection.TryGet(join.Name, out IDatabaseObjectInfo? relatedObjectInfo))
                        throw new NotFoundException(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                    if (relatedObjectInfo == null)
                        throw new NotFoundException(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

                    var joinColumnPropertyInfo = relatedObjectInfo.PropertyInfos.FirstOrDefault(item => item.IsPrimaryKey)?.Clone() ?? throw new NotFoundException(@join.Name, "Primary_Key", ExceptionCode.DatabaseQueryJoiningGenerator); ;

                    joinColumnPropertyInfo.Parent = @join;

                    var indexer = index > 0 ? $"_{index}" : "";
                    var joinType = (@join.Required ?? false) ? JoinType.Inner : JoinType.Left;
                    var expression = joinColumnPropertyInfo.GenerateJoinExpression(propertyObjectInfo.ObjectType, joinType, indexer);
                    queryObjectJoining.Add(expression, joinType, order);

                    index++;
                }

            });

        var joinPredicates = queryObjectJoining.Validate().Return().OrderByDescending(item => item.Order);
        var databaseJoinClauses = new List<DatabaseJoinsClauseQueryPart>();
        foreach (var join in joinPredicates)
        {
            joinGenerator.AddOption("JoinType", join.Type);
            databaseJoinClauses.Add(joinGenerator.Generate(join.PropertyExpression));
            joinGenerator.RemoveOption("JoinType");
        }

        QueryPartCollection.JoinClause = DatabaseJoinsClauseQueryPart.CreateMerged(databaseJoinClauses);

        return Task.CompletedTask;
    }

    protected override Task GenerateGroupByClause()
    {
        var columnQueryPart = QueryPartCollection.Columns ?? throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);
        var whereQueryPart = QueryPartCollection.WhereClause;

        var grouping = false;
        ICollection<WhereClause>? havingClauses = null;
        if (whereQueryPart != null)
        {
            havingClauses = DatabaseWhereClauseQueryPart.GeHavingClause(whereQueryPart.Parameter);
            grouping = havingClauses.Count > 0;
        }

        if (grouping)
        {
            var groupingColumns = columnQueryPart.Parameter.Where(item => item.AggregationFunctionType == AggregationFunctionType.None) ?? throw new NotFoundException(ExceptionCode.DatabaseQueryGroupByGenerator);
            var groupClauses = DatabaseGroupByClauseQueryPart.Create(new GroupingPredicate(groupingColumns, havingClauses));

            groupClauses.GroupingText();
            groupClauses.HavingText();

            QueryPartCollection.GroupByClause = groupClauses;
        }

        return Task.CompletedTask;
    }
}
