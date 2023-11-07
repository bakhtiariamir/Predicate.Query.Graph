using Priqraph.Contract;
using System.Data.SqlClient;

namespace Priqraph.Manager.Result.Database;

public class SqlObjectQuery : ObjectQuery<SqlParameter>, ISqlQuery
{
    public string Phrase
    {
        get;
    }

    public SqlObjectQuery(ICollection<SqlParameter>? parameters, string phrase) : base(parameters)
    {
        Phrase = phrase;
    }

    public override void UpdateParameter(string type, params ParameterValue[] parameters) => Parameters?.ToList().ForEach(parameter =>
    {
        ParameterValue? newParam;
        switch (type)
        {
            case "command":
                newParam = parameters.FirstOrDefault(item => string.Equals(parameter.ParameterName, $"@{item.Name}", StringComparison.CurrentCultureIgnoreCase));
                break;
            default:
                newParam = parameters.FirstOrDefault(item => string.Equals(parameter.ParameterName, item.Name, StringComparison.CurrentCultureIgnoreCase));
                break;
        }

        if (newParam != null)
            parameter.Value = newParam.Value;
    });
}