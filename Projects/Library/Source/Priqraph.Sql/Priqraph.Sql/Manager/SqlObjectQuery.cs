using Priqraph.Contract;
using Priqraph.Manager.Result;
using Priqraph.Sql.Contracts;
using System.Data.SqlClient;

namespace Priqraph.Sql.Manager;

public class SqlObjectQuery(ICollection<SqlParameter>? parameters, string phrase) : ObjectQuery<SqlParameter>(parameters)
{
    public string Phrase
    {
        get;
    } = phrase;
    
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