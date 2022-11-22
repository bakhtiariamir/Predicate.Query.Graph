using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.ExpressionHandler.Visitors
{
    public abstract class DatabaseVisitor<TObject, TResult> : Visitor<TResult, IDatabaseObjectInfo, IDatabaseCacheInfoCollection, IColumnPropertyInfo> where TObject : class where TResult : class, new()
    {

    }
}
