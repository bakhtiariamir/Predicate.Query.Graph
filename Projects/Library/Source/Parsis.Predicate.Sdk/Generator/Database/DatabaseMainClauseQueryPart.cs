using System.Data.SqlClient;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public class DatabaseMainClauseQueryPart<TObject> : DatabaseQueryPart<TObject, SqlParameter> where TObject : class
{
    public override string Text
    {
        get;
        set;
    } = string.Empty;

    protected override QueryPartType QueryPartType => QueryPartType.Main;
}
