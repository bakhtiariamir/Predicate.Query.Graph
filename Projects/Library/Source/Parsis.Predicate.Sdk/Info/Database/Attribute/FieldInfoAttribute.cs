using System.Data;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database.Attribute;
public class FieldInfoAttribute : System.Attribute
{
    public string Name
    {
        get;
        set;
    }

    public PropertyDataType DataType
    {
        get;
        set;
    }

    public string Alias
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    }


    public FieldInfoAttribute(string name, PropertyDataType dataType, string @alias = "", string title = "")
    {
        Name = name;
        DataType = dataType;
        Alias = alias;
        Title = title;
    }

}
