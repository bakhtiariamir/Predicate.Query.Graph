using Parsis.Predicate.Sdk.Contract;
using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerJoiningGenerator : SqlServerVisitor<DatabaseJoinsClauseQueryPart>
{
    public SqlServerJoiningGenerator(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseJoinsClauseQueryPart VisitMember(MemberExpression expression)
    {
        IColumnPropertyInfo[] fields = expression.GetProperty(ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        var field = fields.FirstOrDefault();

        if (!CacheObjectCollection.TryGet(field.Name, out IDatabaseObjectInfo? joinPropertyInfo))
            throw new Parsis.Predicate.Sdk.Exception.NotFoundException(ExceptionCode.DatabaseQueryJoiningGenerator);

        JoinType joinType = JoinType.Inner;
        if (GetOption("JoinType", out object? joinTypeObject))
            joinType = (JoinType)joinTypeObject;

        return DatabaseJoinsClauseQueryPart.Create(new JoinPredicate(field, joinType, joinPropertyInfo));
    }
}
