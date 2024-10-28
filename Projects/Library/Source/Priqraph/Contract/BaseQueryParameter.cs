namespace Priqraph.Contract
{
    public class BaseQueryParameter
    {
        public string Name
        {
            get;
        }

        public object? Value
        {
            get;
            set;
        }

        public BaseQueryParameter(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}