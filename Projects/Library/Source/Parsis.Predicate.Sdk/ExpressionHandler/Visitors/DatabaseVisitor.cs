using System.Linq.Expressions;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Generator.Database;

namespace Parsis.Predicate.Sdk.ExpressionHandler.Visitors;

public abstract class DatabaseVisitor<TResult> : Visitor<TResult, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TResult : IDatabaseQueryPart
{
    protected DatabaseVisitor(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression)
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