using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Helper;
using System.Linq.Expressions;

namespace Priqraph.Generator.Database.Visitors;

public class JoinVisitor : DatabaseVisitor<JoinQueryFragment>
{
    public JoinVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override JoinQueryFragment VisitMember(MemberExpression expression)
    {
        var fields = GetProperty(expression, ObjectInfo, CacheObjectCollection, true)?.ToArray() ?? throw new NotFound(ExceptionCode.DatabaseQuerySelectingGenerator);
        var field = fields.FirstOrDefault() ?? throw new ArgumentNullException(expression.Member.Name);


        if (!CacheObjectCollection.TryGetLastDatabaseObjectInfo(field.Type, out var joinPropertyInfo) || joinPropertyInfo is null)
            throw new NotFound(ExceptionCode.DatabaseQueryJoiningGenerator);

        var joinType = JoinType.Inner;
        if (GetOption("JoinType", out var joinTypeObject) && joinTypeObject != null)
            joinType = (JoinType)joinTypeObject;

        return JoinQueryFragment.Create(new JoinProperty(field, joinType, joinPropertyInfo));
    }
}
