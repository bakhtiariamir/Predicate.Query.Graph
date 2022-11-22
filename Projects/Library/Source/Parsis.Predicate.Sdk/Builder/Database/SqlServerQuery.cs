using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Generator.Database;
using Parsis.Predicate.Sdk.Generator.Database.SqlServer;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Query;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQuery<TObject> : DatabaseQuery<TObject> where TObject : class
{
    private IDatabaseObjectInfo _objectInfo;
    public SqlServerQuery(IQueryContext context, DatabaseQueryOperationType queryType) : base(context, queryType) => _objectInfo = Context.DatabaseCacheInfoCollection?.GetLastObjectInfo<TObject>() ?? throw new NotFoundException(typeof(TObject).Name, ExceptionCode.DatabaseObjectInfo);

    protected Task GenerateInitializing()
    {
        throw new NotImplementedException();
    }

    public Task GenerateGrouping()
    {
        throw new NotImplementedException();
    }

    protected override Task GenerateColumn(QueryObject<TObject, DatabaseQueryOperationType> query)
    {

        var fieldsExpression = query.Columns?.ToList();
        var parameterExpression = fieldsExpression[0].Expression.Parameters[0];
        var selectingGenerator = new SqlServerSelectingGenerator<TObject>(Context.DatabaseCacheInfoCollection, _objectInfo, parameterExpression);
        var columns = new List<DatabaseColumnsClauseQueryPart<TObject>>();
        fieldsExpression.ToList().ForEach(field =>
        {
            Expression? expression = null;
            if (field.Expression != null)
            { 
                expression = ((LambdaExpression)field.Expression).Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            }

            if (field.Expressions != null)
            {
                expression = ((LambdaExpression)field.Expressions).Body ?? throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);
            }
            if (expression == null)
                throw new NotFoundException(typeof(TObject).Name, "Expression.Body", ExceptionCode.DatabaseQueryFilteringGenerator);

            var property = selectingGenerator.Generate(expression);
            columns.Add(property);
        });

        var queryColumns = DatabaseColumnsClauseQueryPart<TObject>.CreateMerged(columns.Select(item => item));
        QueryPartCollection.Columns = queryColumns;

        var joins = new List<Tuple<IColumnPropertyInfo, int>>();

        Action<ICollection<IColumnPropertyInfo>, int>? getJoins = null;
        getJoins = (parameters, level) =>
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Parent is not null && !parameter.IsPrimaryKey)
                    if (joins.All(item => !item.Item1.Equals(parameter.Parent)))
                    {
                        joins.Add(new Tuple<IColumnPropertyInfo, int>(parameter.Parent, level));
                        getJoins?.Invoke(new[]
                        {
                                parameter.Parent
                        }, ++level);
                    }
            }
        };

        getJoins(queryColumns.Parameters, 0);

        joins.OrderByDescending(item => item.Item2).
            Select(item => item.Item1)
            .GroupBy(item => new { item.Schema, item.DataSet, item.ColumnName, item.Name }).ToList().ForEach(item =>
       {
            var index = 0;

            foreach (var join in item)
            {
                if (!Context.DatabaseCacheInfoCollection.TryGet(join.DataSet, out IDatabaseObjectInfo? propertyObjectInfo))
                    throw new NotFoundException(join.DataSet, ExceptionCode.DatabaseQuerySelectingGenerator);

                if (propertyObjectInfo == null)
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQuerySelectingGenerator);

                if (!Context.DatabaseCacheInfoCollection.TryGet(join.Name, out IDatabaseObjectInfo? relatedObjectInfo))
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQuerySelectingGenerator);

                if (relatedObjectInfo == null)
                    throw new NotFoundException(join.Name, ExceptionCode.DatabaseQuerySelectingGenerator);

                var primaryKey = relatedObjectInfo.PropertyInfos.FirstOrDefault(item => item.IsPrimaryKey) ?? throw new NotFoundException(join.Name, "Primary_key", ExceptionCode.DatabaseQuerySelectingGenerator);

                var indexer = index > 0 ? $"_{index}" : "";
                var expression = @join.GenerateGetPropertyExpression(propertyObjectInfo, indexer);
                QueryObjectJoining.Add(expression, primaryKey, (@join.Required ?? false) ? JoinType.Inner : JoinType.Left);

                index++;
            }

        });

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
            var whereGenerator = new SqlServerFilteringGenerator<TObject>(Context.DatabaseCacheInfoCollection, _objectInfo, parameterExpression);
            var items = whereGenerator.Generate(body);
        }

        return Task.CompletedTask;
    }

    protected override Task GeneratePagingClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        throw new NotImplementedException();
    }

    protected override Task GenerateOrderByClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        throw new NotImplementedException();
    }

    protected override Task GenerateJoinClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        throw new NotImplementedException();
    }

    protected override Task GenerateGroupByClause(QueryObject<TObject, DatabaseQueryOperationType> query)
    {
        throw new NotImplementedException();
    }
}
