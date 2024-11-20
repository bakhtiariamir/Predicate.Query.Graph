namespace Priqraph.Setup
{
    public class SelectConfig
    {
        public ArrayParameter NumberArray
        {
            get;
            set;
        } = ArrayParameter.PassInQuery;

        public ArrayParameter StringArray
        {
            get;
            set;
        } = ArrayParameter.PassInQuery;
    }
}