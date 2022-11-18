using Parsis.Predicate.Sdk.Contract;

namespace Parsis.Predicate.Sdk.ExpressionHandler.Visitors
{
    public abstract class DatabaseVisitor<TObject, TResult> : Visitor<TObject, TResult, IDatabaseQueryContext<TObject>> where TObject : class
    {

    }
}
