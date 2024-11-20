using System.Data;

namespace Priqraph.DataType
{
    public class Neo4JParameter
    {
        public Neo4JParameter(string parameterName, ColumnDataType? dataType)
        {
            ParameterName = parameterName;
            DataType = dataType;
        }

        public Neo4JParameter(string parameterName, object? value)
        {
            ParameterName = parameterName;
            Value = value;
        }
        
        public Neo4JParameter(string parameterName, object? value, string typeName)
        {
            ParameterName = parameterName;
            TypeName = typeName;
            Value = value;
        }
        
        public string? ParameterName
        {
            get;
            set;
        } 
        
        public object? Value
        {
            get;
            set;
        }

        public ColumnDataType? DataType
        {
            get;
            set;
        } = null;
        
        public string TypeName
        {
            get;
            set;
        }
    }
}