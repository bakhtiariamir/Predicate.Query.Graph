namespace Priqraph.Exception
{
    public class NotValidException : BaseException 
    {
        public NotValidException(string code) : base(code)
        {
        }

        public NotValidException(string code, string? message) : base(code, message)
        {
        }
    }
}