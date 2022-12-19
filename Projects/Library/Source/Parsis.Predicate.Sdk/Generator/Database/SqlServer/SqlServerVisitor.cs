using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.ExpressionHandler;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;

public abstract class SqlServerVisitor<TResult> : Visitor<TResult, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TResult : IDatabaseQueryPart
{
    protected SqlServerVisitor(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression)
    {
        CacheObjectCollection = cacheObjectCollection;
        ObjectInfo = objectInfo;
        ParameterExpression = parameterExpression;
    }

    protected override IDatabaseCacheInfoCollection CacheObjectCollection
    {
        get;
    }

    protected override IDatabaseObjectInfo ObjectInfo
    {
        get;
    }

    protected override ParameterExpression? ParameterExpression
    {
        get;
    }

    protected override TResult VisitConvert(UnaryExpression expression)
    {
        return Visit(expression.Operand);
    }
}