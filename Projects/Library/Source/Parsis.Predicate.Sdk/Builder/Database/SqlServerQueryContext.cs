using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Helper;

namespace Parsis.Predicate.Sdk.Builder.Database;
public class SqlServerQueryContext<TObject> : DatabaseQueryContext<TObject>, ISqlServerQueryContext<TObject> where TObject : class
{
}