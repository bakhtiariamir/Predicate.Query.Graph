using Priqraph.Builder.Database;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;
using System.Data.SqlClient;

namespace Priqraph.Manager.Result.Database;

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

    public override void UpdateParameter(string type, params ParameterValue[] parameters) => Parameters?.ToList().ForEach(parameter =>
    {
        ParameterValue? newParam;
        if  (type == "command")
            newParam = parameters.FirstOrDefault(item => string.Equals(parameter.ParameterName, $"@{item.Name}", StringComparison.CurrentCultureIgnoreCase));
        else 
            newParam = parameters.FirstOrDefault(item => string.Equals(parameter.ParameterName, item.Name, StringComparison.CurrentCultureIgnoreCase));

        if (newParam != null)
            parameter.Value = newParam.Value;
    });
}

public class SqlObjectQueryGenerator : IObjectQueryGenerator<SqlParameter, SqlObjectQuery, DatabaseQueryPartCollection>
{
    public SqlObjectQuery? GenerateResult(QueryOperationType operationType, DatabaseQueryPartCollection query)
    {
        var phrase = string.Empty;
        ICollection<SqlParameter>? parameters = null;
        switch (operationType)
        {
            case QueryOperationType.GetData:
                phrase = query.GetSelectQuery(out parameters);
                break;
            case QueryOperationType.Add:
            case QueryOperationType.Edit:
            case QueryOperationType.Remove:
            case QueryOperationType.Merge:
                phrase = query.GetCommandQuery(out parameters);
                break;
        }

        return new SqlObjectQuery(operationType, parameters, phrase);
    }
}
