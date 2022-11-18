using System.Data.SqlClient;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Generator.Database;
public class DatabaseWhereClauseQueryPart<TObject, TParameter> : DatabaseQueryPart<TObject, TParameter> where TObject : class
{

    private string _text;
    public override string Text
    {
        get => _text; 
        set => _text = value;
    }

    protected override QueryPartType QueryPartType => QueryPartType.Where;


    private DatabaseWhereClauseQueryPart(string text, IEnumerable<TParameter> parameters)
    {
        _text = text;
        Parameters = parameters;
    }

    public static DatabaseWhereClauseQueryPart<TObject, TParameter> Create(string text, params TParameter[] parameters) => new(text, parameters);

    public static DatabaseWhereClauseQueryPart<TObject, TParameter> Create(params TParameter[] parameters) => new(string.Join(", ", parameters.Select(parameter => parameter.ToString())), parameters);

    public static DatabaseWhereClauseQueryPart<TObject, TParameter> CreateMerged(string text, params DatabaseWhereClauseQueryPart<TObject, TParameter>[] sqlClauses) => new(text, sqlClauses.SelectMany(parameters => parameters.Parameters));

    public static DatabaseWhereClauseQueryPart<TObject, TParameter> CreateMerged(string text, IEnumerable<DatabaseWhereClauseQueryPart<TObject, TParameter>> sqlQueries) => new(text, sqlQueries.SelectMany(parameters => parameters.Parameters));
}
