using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Manager.Result;
using Priqraph.Neo4j.Contracts;

namespace Priqraph.Neo4j.Manager;

public class Neo4jObjectQuery(ICollection<Neo4JParameter>? parameters, string phrase) : ObjectQuery<Neo4JParameter>(parameters), INeo4jQuery
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