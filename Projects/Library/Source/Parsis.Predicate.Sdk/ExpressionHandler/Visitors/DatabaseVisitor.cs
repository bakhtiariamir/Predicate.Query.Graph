using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.ExpressionHandler.Visitors
{
    public abstract class DatabaseVisitor<TObject, TResult> : Visitor<TResult, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TObject : IQueryableObject where TResult : class, new()
    {

    }
}
