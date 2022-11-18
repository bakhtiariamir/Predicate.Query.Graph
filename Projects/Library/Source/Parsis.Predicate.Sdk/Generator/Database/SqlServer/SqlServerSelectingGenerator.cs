using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerSelectingGenerator<TObject> : Visitor<TObject, DatabaseColumnsClauseQueryPart<TObject>, IDatabaseQueryContext<TObject>> where TObject : class
{

    protected override IDatabaseQueryContext<TObject> QueryContext
    {
        get;
    }

    protected override ParameterExpression ParameterExpression
    {
        get;
    }

    public SqlServerSelectingGenerator(IDatabaseQueryContext<TObject> queryContext, ParameterExpression parameterExpression)
    {
        QueryContext = queryContext;
        ParameterExpression = parameterExpression;
    }

    public DatabaseColumnsClauseQueryPart<TObject> Generate(Expression expression)
    {
        return Visit(expression);
    }
}
