namespace Priqraph.Contract
{
    /// <summary>
    /// ParameterValue is main object for create query parameter that these object based on query provider convert to provider parameter like SqlParameter.
    /// </summary>
    public class ParameterValue
    {
        public string? Name
        {
            get;
            set;
        }

        public object? Value
        {
            get;
            set;
        }
    }
}
