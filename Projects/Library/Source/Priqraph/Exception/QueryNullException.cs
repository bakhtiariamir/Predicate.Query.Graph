namespace Priqraph.Exception
{
    public class QueryNullException : BaseException
    {
        public QueryNullException(string code) : base(code)
        {
            
        }

        public QueryNullException(string code, string? message) : base(code, message)
        {
            
        }
    }
}