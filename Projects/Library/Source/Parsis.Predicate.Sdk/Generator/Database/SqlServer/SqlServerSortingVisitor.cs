using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerSortingVisitor : DatabaseVisitor<DatabaseOrdersByClauseQueryPart>
{
    public SqlServerSortingVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseOrdersByClauseQueryPart VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }

    protected override DatabaseOrdersByClauseQueryPart VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }

    protected override DatabaseOrdersByClauseQueryPart VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseOrdersByClauseQueryPart.Create(fields.Select(item => new ColumnSortPredicate(item)).ToArray());
    }
}
