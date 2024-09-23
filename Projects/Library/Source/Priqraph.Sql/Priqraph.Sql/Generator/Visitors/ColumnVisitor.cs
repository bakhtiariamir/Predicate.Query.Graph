using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Priqraph.Sql.Generator.Visitors;
public class ColumnVisitor(
    ICacheInfoCollection cacheObjectCollection,
    IDatabaseObjectInfo objectInfo,
    ParameterExpression? parameterExpression)
    : DatabaseVisitor<ColumnQueryFragment>(cacheObjectCollection, objectInfo, parameterExpression)
{
    protected override ColumnQueryFragment VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }

    protected override ColumnQueryFragment VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }

    protected override ColumnQueryFragment VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }
}
