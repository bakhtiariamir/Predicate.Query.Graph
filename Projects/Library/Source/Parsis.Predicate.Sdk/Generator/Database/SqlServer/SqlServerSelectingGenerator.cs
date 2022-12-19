using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public class SqlServerSelectingGenerator : SqlServerVisitor<DatabaseColumnsClauseQueryPart>
{
    public SqlServerSelectingGenerator(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }



    protected override DatabaseColumnsClauseQueryPart VisitMember(MemberExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart VisitParameter(ParameterExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }

    protected override DatabaseColumnsClauseQueryPart VisitNewArray(NewArrayExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        return DatabaseColumnsClauseQueryPart.Create(fields);
    }
}
