using System.Data.SqlClient;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerFilteringGenerator<TObject> : Visitor<TObject, DatabaseWhereClauseQueryPart<TObject, SqlParameter>, IDatabaseQueryContext<TObject>> where TObject : class
{

    protected override IDatabaseQueryContext<TObject> QueryContext
    {
        get;
    }

    protected override ParameterExpression ParameterExpression
    {
        get;
    }

    public SqlServerFilteringGenerator(IDatabaseQueryContext<TObject> queryContext, ParameterExpression parameterExpression)
    {
        QueryContext = queryContext;
        ParameterExpression = parameterExpression;
    }

    public DatabaseWhereClauseQueryPart<TObject, SqlParameter> Generate(Expression expression) => Visit(expression);
    
    protected override DatabaseWhereClauseQueryPart<TObject, SqlParameter> VisitAndAlso(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return DatabaseWhereClauseQueryPart<TObject, SqlParameter>.CreateMerged($"{left} AND {right}", new[] {left, right});
    }

    protected override DatabaseWhereClauseQueryPart<TObject, SqlParameter>  VisitOrElse(BinaryExpression expression)
    {
        var left = Visit(expression.Left);
        var right = Visit(expression.Right);
        return DatabaseWhereClauseQueryPart<TObject, SqlParameter>.CreateMerged($"{left} OR {right}", new[] { left, right });
    }

    protected override DatabaseWhereClauseQueryPart<TObject, SqlParameter> VisitEqual(BinaryExpression expression)
    {
        //ToDo : Implement recursive
        /*
         * Select * from Portfolio
         * Where Portfolio.AccountId in (
         * Select Id from Account
         * Inner join User on User.Id = Account.UserId
         * Where User.Username in ("Ali", "Pedram", "Saman")
         * )
         */
        return base.VisitEqual(expression);
    }
}
