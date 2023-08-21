using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Priqraph.Generator.Database.SqlServer;

public class SqlServerSelectingVisitor : DatabaseVisitor<DatabaseColumnsClauseQueryPart>
{
    public SqlServerSelectingVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseColumnsClauseQueryPart VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }
}
