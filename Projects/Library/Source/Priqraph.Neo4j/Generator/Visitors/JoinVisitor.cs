using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Helper;
using System.Linq.Expressions;
using Priqraph.Generator;
using Priqraph.Generator.Database;

namespace Priqraph.Neo4j.Generator.Visitors;
public class JoinVisitor(
    ICacheInfoCollection cacheObjectCollection,
    IDatabaseObjectInfo objectInfo,
    ParameterExpression? parameterExpression)
    : DatabaseVisitor<Neo4JJoinQueryFragment>(cacheObjectCollection, objectInfo, parameterExpression)
{
    protected override Neo4JJoinQueryFragment VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFoundException(ExceptionCode.DatabaseQuerySelectingGenerator);
        var field = fields.FirstOrDefault() ?? throw new ArgumentNullException(expression.Member.Name);


        if (!CacheObjectCollection.TryGetLastDatabaseObjectInfo(field.Type, out var joinPropertyInfo) || joinPropertyInfo is null)
            throw new NotFoundException(ExceptionCode.DatabaseQueryJoiningGenerator);

        var joinType = JoinType.Inner;
        if (GetOption("JoinType", out var joinTypeObject) && joinTypeObject != null)
            joinType = (JoinType)joinTypeObject;

        return Neo4JJoinQueryFragment.Create(new JoinProperty(field, joinType, joinPropertyInfo));
    }
}
