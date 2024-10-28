using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Priqraph.Neo4j.Generator.Visitors;
public class ColumnVisitor(
    ICacheInfoCollection cacheObjectCollection,
    IDatabaseObjectInfo objectInfo,
    ParameterExpression? parameterExpression)
    : DatabaseVisitor<Neo4jColumnQueryFragment>(cacheObjectCollection, objectInfo, parameterExpression)
{
    protected override Neo4jColumnQueryFragment VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jColumnQueryFragment.Create(fields);
    }

    protected override Neo4jColumnQueryFragment VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jColumnQueryFragment.Create(fields);
    }

    protected override Neo4jColumnQueryFragment VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return Neo4jColumnQueryFragment.Create(fields);
    }
}
