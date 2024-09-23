using Priqraph.Contract;

namespace Priqraph.Test.Query
{
    public class Person : IQueryableObject
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        } = string.Empty;

        public string LastName
        {
            get;
            set;
        } = string.Empty;
        
        public string NationalCode
        {
            get;
            set;
        } = string.Empty;
        
        public DateTime BirthDate
        {
            get;
            set;
        }
        
        public DateTime? DeathDate
        {
            get;
            set;
        }
    }
}