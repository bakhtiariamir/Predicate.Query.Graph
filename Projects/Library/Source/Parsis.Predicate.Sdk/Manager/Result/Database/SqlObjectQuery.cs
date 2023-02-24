using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using System.Data.SqlClient;

namespace Parsis.Predicate.Sdk.Manager.Result.Database;

public class SqlObjectQuery : ObjectQuery<SqlParameter>, ISqlQuery
{
    public string Phrase
    {
        get;
    }

    public SqlObjectQuery(QueryOperationType queryOperationType, ICollection<SqlParameter>? parameters, string phrase) : base(queryOperationType, parameters)
    {
        Phrase = phrase;
    }

    public override void UpdateParameter(params ParameterValue[] parameters) => Parameters?.ToList().ForEach(parameter =>
    {
        var newParam = parameters.FirstOrDefault(item => string.Equals(parameter.ParameterName, item.Name, StringComparison.CurrentCultureIgnoreCase));
        if (newParam != null)
            parameter.Value = newParam.Value;
    });
}

public class SqlObjectQueryGenerator : IObjectQueryGenerator<SqlParameter, SqlObjectQuery, DatabaseQueryPartCollection>
{
    //public ISqlQuery? GenerateResult(QueryOperationType operationType, DatabaseQueryPartCollection query) => throw new NotImplementedException();

    public SqlObjectQuery? GenerateResult(QueryOperationType operationType, DatabaseQueryPartCollection query)
    {
        var phrase = string.Empty;
        ICollection<SqlParameter>? parameters = null;
        switch (operationType)
        {
            case QueryOperationType.GetData:
                phrase = query.GetSelectQuery(out parameters);
                break;
            //case QueryOperationType.Add:
            //    phrase = query.GetCommandQuery()
        }

        return new SqlObjectQuery(operationType, parameters, phrase);
    }
}
