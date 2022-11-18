using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Sdk.Builder.Database;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Query;

namespace Parsis.Predicate.Console.Query;
public class PersonDatabaseQuery : PersonQuery<DatabaseQueryPartCollection>
{
    private QueryObject<Person>? _queryObject;

    protected override QueryObject<Person>? QueryObject
    {
        get => _queryObject;
    }
    protected override IQueryBuilder<Person, DatabaseQueryPartCollection> QueryBuilder
    {
        get;
    }


    public PersonDatabaseQuery(IDatabaseCacheObjectInfo<Person> cachePersonInfo)
    {
        QueryBuilder = new SqlServerQueryBuilder<Person>(cachePersonInfo);
    }

    public override async Task<string> SelectQueryAsync()
    {
        _queryObject = QueryObjectBuilder<Person>.Init(QueryType.Select).
            SetSelecting(QueryObjectSelecting<Person>.Init().Add(item => item.Id).Add(item => item.Age).Validation()).
            SetFiltering(QueryObjectFiltering<Person>.Init(item => item.Age >= 12).And(item => item.Age <= 25).Validation()).
            SetSorting(QueryObjectSorting<Person>.Init().Add(item => item.Age, DirectionType.Asc).Validation()).
            Generate();
        await base.SelectQueryAsync();

        if (Query == null)
            throw new Exception("123");

        return (await Query.Build()).GetSelectQuery();
    }

}


