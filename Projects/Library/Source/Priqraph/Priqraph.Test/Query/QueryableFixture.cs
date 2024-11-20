using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Query;
using Priqraph.Setup;

namespace Priqraph.Test.Query
{
    public class QueryableFixture : IDisposable
    {
        public QueryBuilder<Person, ISqlQuery<Person, DatabaseQueryOperationType>, DatabaseQueryOperationType> QueryBuilder
        {
            get;
            set;
        } = new();

        private readonly List<Person> _persons = new()
        {
            new Person{ Id = 1, Name = "John", LastName = "Johny", BirthDate = DateTime.Now.AddYears(-20), NationalCode = "1234567890"}
        };
        

        public QueryableFixture()
        {
        }

        public void InitQuery() => QueryBuilder.Init(DatabaseQueryOperationType.Add, QueryProvider.SqlServer, default);

        public void SetQuery() => QueryBuilder.SetQuery((from x in _persons select x).AsQueryable());

        public void Dispose() => _persons.Clear();
    }
}