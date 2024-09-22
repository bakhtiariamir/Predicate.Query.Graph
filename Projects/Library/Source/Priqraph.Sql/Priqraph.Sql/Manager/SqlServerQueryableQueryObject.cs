using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Generator.Database;
using Priqraph.Helper;
using Priqraph.Query;
using Priqraph.Query.Builders;
using Priqraph.Setup;
using System.Linq.Expressions;
using Priqraph.Builder.Database;
using Priqraph.Sql.Generator;
using Priqraph.Sql.Generator.Visitors;

namespace Priqraph.Sql.Manager;

internal class SqlServerQueryableQueryObject<TObject> : DatabaseQueryableQueryObject<TObject>, ISqlServerQueryObject<TObject> where TObject : IQueryableObject
{
    private IDatabaseObjectInfo _objectInfo;

    public SqlServerQueryableQueryObject(ICacheInfoCollection cacheInfoCollection) : base(cacheInfoCollection)
    {
        _objectInfo = cacheInfoCollection?.LastDatabaseObjectInfo<TObject>() ?? throw new NotFound(typeof(TObject).Name, "", ExceptionCode.DatabaseObjectInfo);
        QueryResult.DatabaseObjectInfo = _objectInfo;
    }

    //protected override void GenerateInsert(IQueryObject<TObject> query)
    //{
    //    var command = query.CommandPredicates ?? throw new NotSupported("a");
    //    var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
    //    GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Add);

    //    if (QueryResult.CommandFragment != null && command.ReturnType == ReturnType.Record && QueryResult.CommandFragment.CommandParts.ContainsKey("result"))
    //    {
    //        if (query.ColumnPredicates == null)
    //            throw new ArgumentNullException($"Columns for insert query in return record mode for {typeof(TObject).Name} can not be null.");

    //        var builder = new QueryObjectBuilder<TObject>(QueryProvider.SqlServer);
    //        builder.Init(QueryOperationType.GetData, QueryProvider.SqlServer, query.ObjectTypeStructures);
    //        builder.Init(QueryOperationType.GetData, QueryProvider.SqlServer, query.ObjectTypeStructures);
    //        var selectQuery = builder.SetColumns(query.ColumnPredicates).SetFilter(FilterPredicateBuilder<TObject>.Init(ReturnType.Record).Return()).Generate();
    //        var sqlQuery = Build(selectQuery);
    //        QueryResult.ResultQuery = sqlQuery;
    //    }
    //}

    //protected override void GenerateUpdate(IQueryObject<TObject> query)
    //{
    //    var command = query.CommandPredicates ?? throw new NotSupported("a");
    //    var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
    //    GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Edit);
    //    if (QueryResult.CommandFragment != null && command.ReturnType == ReturnType.Record && QueryResult.CommandFragment.CommandParts.ContainsKey("result"))
    //    {
    //        if (query.ColumnPredicates == null)
    //            throw new ArgumentNullException($"Columns for update query in return record mode for {typeof(TObject).Name} can not be null.");

    //        var builder = new QueryObjectBuilder<TObject>(QueryProvider.SqlServer);
    //        builder.Init(QueryOperationType.GetData, QueryProvider.SqlServer, query.ObjectTypeStructures);
    //        var selectQuery = builder.SetColumns(query.ColumnPredicates).SetFilter(FilterPredicateBuilder<TObject>.Init(ReturnType.Record).Return()).Generate();
    //        var sqlQuery = Build(selectQuery);
    //        QueryResult.ResultQuery = sqlQuery;
    //    }
    //}

    //protected override void GenerateDelete(IQueryObject<TObject> query)
    //{
    //    var command = query.CommandPredicates ?? throw new NotSupported("a");
    //    var commandSqlVisitor = new CommandVisitor(Context.CacheInfoCollection, _objectInfo, null);
    //    GenerateRecordCommand(command, commandSqlVisitor, QueryOperationType.Remove);

    //}

    protected override void GenerateSelect(IQuery<TObject> query)
    {
	    var expression = query.Queryable?.Expression ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
	    var parameter = Expression.Parameter(typeof(TObject));
	    var visitor = new QueryableVisitor<TObject>(parameter, Context.CacheInfoCollection, _objectInfo);
	    visitor.Generate(expression);
    }

    //protected override void GenerateCount(IQueryObject<TObject> query)
    //{
	   // throw new NotImplementedException();
    //}

    //protected override void GenerateColumn(IQueryObject<TObject> query, bool getCount = false)
    //{
    //    if (!getCount)
    //    {
    //        var fieldsExpression = query.ColumnPredicates?.ToList();
    //        ParameterExpression? parameterExpression = null;
    //        if (fieldsExpression[0]?.Expression != null)
    //        {
    //            parameterExpression = fieldsExpression[0].Expression?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
    //        }
    //        else if (fieldsExpression[0]?.Expressions != null)
    //        {
    //            parameterExpression = fieldsExpression[0].Expressions?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
    //        }

    //        var selectingGenerator = new ColumnVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
    //        var columns = new List<ColumnQueryFragment>();
    //        fieldsExpression.ToList().ForEach(field =>
    //        {
    //            Expression? expression = null;
    //            if (field.Expression != null)
    //                expression = field.Expression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

    //            if (field.Expressions != null)
    //                expression = field.Expressions.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

    //            if (expression == null)
    //                throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

    //            var property = selectingGenerator.Generate(expression);
    //            columns.Add(property);
    //        });

    //        var queryColumns = ColumnQueryFragment.Merged(columns.Select(item => item));
    //        QueryResult.ColumnFragment = queryColumns;
    //        JoinColumns.AddRange(queryColumns.Parameter);
    //    }
    //    else
    //    {
    //        QueryResult.ColumnFragment = ColumnQueryFragment.CreateCount();
    //    }


    //}

    //protected override void GenerateWhere(IQueryObject<TObject> query)
    //{
    //    if (query.FilterPredicates?.ReturnType != ReturnType.None)
    //    {
    //        var key = _objectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException("Primary key can not be null.");
    //        var clause = new FilterProperty(key, null, ConditionOperatorType.Equal);
    //        switch (query.FilterPredicates?.ReturnType)
    //        {
    //            case ReturnType.Record:
    //                var whereClause = FilterQueryFragment.Create(clause);
    //                whereClause.SetText(ReturnType.Record, clause);
    //                QueryResult.FilterFragment = whereClause;
    //                break;
    //            case ReturnType.None:
    //                break;
    //            case ReturnType.Key:
    //                break;
    //            case ReturnType.RowAffected:
    //                break;
    //            case null:
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException();
    //        }
    //    }

    //    var expression = query.FilterPredicates?.Expression;
    //    if (expression != null)
    //    {
    //        if (expression.NodeType != ExpressionType.Lambda)
    //            throw new NotSupported(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

    //        var lambdaExpression = (LambdaExpression)expression;
    //        var body = lambdaExpression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
    //        var parameterExpression = lambdaExpression.Parameters[0];
    //        var whereGenerator = new FilterVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
    //        var whereClause = whereGenerator.Generate(body);
    //        whereClause.SetQuerySetting(Context.CacheInfoCollection.QuerySetting);
    //        whereClause.ReduceParameter();
    //        whereClause.SetText();
    //        QueryResult.FilterFragment = whereClause;
    //        var joinColumns = FilterQueryFragment.GetColumnProperties(whereClause.Parameter);
    //        JoinColumns.AddRange(joinColumns);
    //    }


    //}

    //protected override void GeneratePaging(IQueryObject<TObject> query)
    //{
    //    var expression = query.PagePredicate?.Predicate;
    //    if (expression != null)
    //    {
    //        if (expression!.NodeType != ExpressionType.Lambda)
    //            throw new NotSupported(typeof(TObject).Name, expression.NodeType.ToString(), ExceptionCode.DatabaseQueryFilteringGenerator);

    //        var lambdaExpression = (LambdaExpression)expression;
    //        var body = lambdaExpression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

    //        var pagingVisitor = new PageVisitor(Context.CacheInfoCollection, _objectInfo, default);
    //        QueryResult.PageFragment = pagingVisitor.Generate(body);
    //    }
    //}

    //protected override void GenerateOrderBy(IQueryObject<TObject> query)
    //{
    //    var sortExpression = query.SortPredicates?.ToList();
    //    if (sortExpression != null)
    //    {
    //        var parameterExpression = sortExpression[0].Expression?.Parameters[0] ?? throw new NotFound(typeof(TObject).Name, "Expression.Parameter", ExceptionCode.DatabaseQueryFilteringGenerator);
    //        var sortingGenerator = new SortVisitor(Context.CacheInfoCollection, _objectInfo, parameterExpression);
    //        sortExpression.ToList().ForEach(sortPredicate =>
    //        {
    //            Expression? expression = null;
    //            if (sortPredicate.Expression != null)
    //            {
    //                expression = sortPredicate.Expression.Body ?? throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
    //            }

    //            if (expression == null)
    //                throw new NotFound(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

    //            var orderByProperty = sortingGenerator.Generate(expression);
    //            QueryResult.SortFragment = QueryResult.SortFragment == null ? orderByProperty : SortQueryFragment.Merged(new[] { (SortQueryFragment)QueryResult.SortFragment, orderByProperty });
    //        });
    //    }
    //}

    //protected override void GenerateJoin(IQueryObject<TObject> query)
    //{
    //    var joins = new List<Tuple<IColumnPropertyInfo, int>>();

    //    var joinGenerator = new JoinVisitor(Context.CacheInfoCollection, _objectInfo, null);

    //    Action<ICollection<IColumnPropertyInfo>, int>? getJoins = null;
    //    getJoins = (parameters, level) =>
    //    {
    //        foreach (var parameter in parameters)
    //        {
    //            if (parameter.Parent is null || parameter.Parent.Name == _objectInfo.DataSet)
    //                continue;

    //            if (joins.Any(item => item.Item1.Equals(parameter.Parent)))
    //                continue;

    //            if (parameter.FieldType == DatabaseFieldType.Related)
    //                continue;

    //            joins.Add(new Tuple<IColumnPropertyInfo, int>(parameter.Parent, level));
    //            getJoins?.Invoke(new[] { parameter.Parent }, ++level);
    //        }
    //    };

    //    getJoins(JoinColumns.Where(item => item.Parent is not null && item.Parent.Name != _objectInfo.DataSet).DistinctBy(item => new { item.Schema, item.DataSet, item.ColumnName, item.Name }).ToList(), 0);
    //    var queryObjectJoining = JoinPredicateBuilder.Init();

    //    joins.OrderBy(item => item.Item2).Select(item => item).GroupBy(item => new { item.Item1.Schema, item.Item1.DataSet, item.Item1.ColumnName, item.Item1.Name }).ToList().ForEach(item =>
    //    {
    //        var index = 0;
    //        foreach (var joinPredicate in item.Select(item => item))
    //        {
    //            var join = joinPredicate.Item1;
    //            var order = joinPredicate.Item2;
    //            if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Parent.Type, out var propertyObjectInfo))
    //                throw new NotFound(@join.DataSet, ExceptionCode.DatabaseQueryJoiningGenerator);

    //            if (propertyObjectInfo == null)
    //                throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

    //            if (!Context.CacheInfoCollection.TryGetLastDatabaseObjectInfo(join.Type, out var relatedObjectInfo))
    //                throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

    //            if (relatedObjectInfo == null)
    //                throw new NotFound(@join.Name, ExceptionCode.DatabaseQueryJoiningGenerator);

    //            var joinColumnPropertyInfo = relatedObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key)?.Clone() ?? throw new NotFound(@join.Name, "Primary_Key", ExceptionCode.DatabaseQueryJoiningGenerator);

    //            joinColumnPropertyInfo.Parent = @join;

    //            var indexer = index > 0 ? $"_{index}" : "";
    //            var joinType = @join.Required ? JoinType.Inner : JoinType.Left;
    //            var expression = joinColumnPropertyInfo.GenerateJoinExpression(propertyObjectInfo.ObjectType, joinType, query.ObjectTypeStructures.ToArray(), indexer);
    //            queryObjectJoining.Add(expression, joinType, order);

    //            index++;
    //        }
    //    });

    //    var joinPredicates = queryObjectJoining.Validate().Return().OrderBy(item => item.Order);
    //    var databaseJoinClauses = new List<JoinQueryFragment>();
    //    foreach (var join in joinPredicates)
    //    {
    //        joinGenerator.AddOption("JoinType", join.Type);
    //        databaseJoinClauses.Add(joinGenerator.Generate(join.PropertyExpression));
    //        joinGenerator.RemoveOption("JoinType");
    //    }

    //    QueryResult.JoinFragment = JoinQueryFragment.Merged(databaseJoinClauses);


    //}


}
