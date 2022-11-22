using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Exception;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerFilteringGenerator<TObject> : Visitor<DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TObject : class
{
    protected override IDatabaseCacheInfoCollection CacheObjectCollection
    {
        get;
    }

    protected override IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    protected override ParameterExpression ParameterExpression
    {
        get;
    }

    public SqlServerFilteringGenerator(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
        ParameterExpression = parameterExpression;
    }

    public DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> Generate(Expression expression) => Visit(expression);

    protected override DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> VisitAndAlso(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>.CreateMerged($"{left} AND {right}", new[] { left, right });
    }

    protected override DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> VisitOrElse(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>.CreateMerged($"{left} OR {right}", new[] { left, right });
    }

    protected override DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> VisitGreaterThan(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>.CreateMerged($"{left} > {right}", new[] { left, right });

    }

    protected override DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> VisitMember(MemberExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(expression.ToString(), expression.Member.Name, ExceptionCode.DatabaseQueryFilteringGenerator);

        return DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>.Create(fields);
    }

    //protected override DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> VisitConstant(ConstantExpression expression, string parameterName)
    //{
    //    return GetValue(expression.Value, parameterName);
    //}

    //private DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo> GetValue(object value, string parameterName = null)
    //{
    //    if (value is bool)
    //        return DatabaseWhereClauseQueryPart<TObject, IColumnPropertyInfo>.Create((bool)value ? "(1 = 1)" : "(1 <> 1)");

    //    if (value is byte)
    //        return SqlClause.Create(context.CreateParameter(value, SqlDbType.TinyInt));

    //    if (value is int)
    //        return SqlClause.Create(context.CreateParameter(value, SqlDbType.Int));

    //    if (value is long)
    //        return SqlClause.Create(context.CreateParameter(value, SqlDbType.BigInt));

    //    if (value is decimal)
    //        return SqlClause.Create(context.CreateParameter(value, SqlDbType.Decimal));

    //    if (value is string)
    //        return SqlClause.Create(context.CreateParameter(value, SqlDbType.NVarChar));

    //    if (value is JalaliDateTime)
    //        return SqlClause.Create(context.CreateParameter(((JalaliDateTime)value).Value, SqlDbType.BigInt));

    //    if (value is IEntity)
    //        return SqlClause.Create(context.CreateParameter(((IEntity)value).Id, SqlDbType.Int));

    //    if (value is IEnumerable)
    //    {
    //        var sqlClauses = ((IEnumerable)value).Cast<object>().Select(item => GetValue(item, context)).ToArray();

    //        var ids = new List<string>();
    //        foreach (var parameters in sqlClauses.Select(x => x.Parameters))
    //        {
    //            foreach (var parameter in parameters)
    //                ids.Add(parameter.Value.ToString());
    //        }

    //        var paramName = parameterName ?? "@ValueList";


    //        return new SqlClause(paramName, new List<SqlParameter>
    //            {
    //                new SqlParameter(paramName, string.Join(",", ids))
    //            });
    //    }

    //    throw new NotSupportedException($"valueType: {value.GetType()}, is not supported.");
    //}

    //protected override DatabaseWhereClauseQueryPart<TObject, SqlParameter> VisitEqual(BinaryExpression expression)
    //{
    //    //ToDo : Implement recursive
    //    /*
    //     * Select * from Portfolio
    //     * Where Portfolio.AccountId in (
    //     * Select Id from Account
    //     * Inner join User on User.Id = Account.UserId
    //     * Where User.Username in ("Ali", "Pedram", "Saman")
    //     * )
    //     */
    //    return base.VisitEqual(expression);
    //}
}
