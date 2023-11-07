using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using System.Linq.Expressions;

namespace Priqraph.Generator.Database.Visitors;

public class ColumnVisitor : DatabaseVisitor<ColumnQueryFragment>
{
    public ColumnVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override ColumnQueryFragment VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }

    protected override ColumnQueryFragment VisitParameter(ParameterExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }

    protected override ColumnQueryFragment VisitNewArray(NewArrayExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        return ColumnQueryFragment.Create(fields);
    }
}
