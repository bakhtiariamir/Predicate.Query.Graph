// See https://aka.ms/new-console-template for more information

using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Parsis.Predicate.Console.Model;
using Parsis.Predicate.Console.Query;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Helper;
using Parsis.Predicate.Sdk.Info;

Console.WriteLine("Start-----");


// Snip : Inject dependency with Autofac
// Warn : 
// Samp : asdas 
// ToDo : 1. Get All Object with GenericType Assembilies and Create Object info and Cache. if has error return exception
// ToDo : 2. Cache Query Structure, after create query for first time before run cache object of query.
new ServiceCollection().AddMemoryCache();
var builder = new Autofac.ContainerBuilder();
builder.RegisterType<DatabaseCacheInfoCollection>().As<IDatabaseCacheInfoCollection>().SingleInstance();
var container = builder.Build();
#region Select Sample

// Snip : Select Query sample on one entity
var databaseCacheInfoCollection = container.Resolve<IDatabaseCacheInfoCollection>();

var personObjectInfo = typeof(Person).GetObjectInfo();
var userObjectInfo = typeof(User).GetObjectInfo();
var statusObjectInfo = typeof(Status).GetObjectInfo();

databaseCacheInfoCollection.InitCache(nameof(User), userObjectInfo);
databaseCacheInfoCollection.InitCache(nameof(Person), personObjectInfo);
databaseCacheInfoCollection.InitCache(nameof(Status), statusObjectInfo);
//ToDo : add validation for init Cache when object info has error in Attribute return exception
//var query = new PersonDatabaseQuery(databaseCacheInfoCollection);

var query = new UserDatabaseQuery(databaseCacheInfoCollection);

var listOfQuery = new Dictionary<string, string>();

listOfQuery.Add("Select", await query.SelectQueryAsync());
listOfQuery.Add("SingleInsert", await query.InsertQueryAsync(new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
{
    Id = 1,
    Name = "test"
}, 12), "asdad", "asdadad", DateTime.Now, true, 12)));

listOfQuery.Add("MultipleInsert", await query.BulkInsertQueryAsync(new[]
{
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12)
}));

listOfQuery.Add("SingleUpdate", await query.UpdateCommandAsync(new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
{
    Id = 1,
    Name = "test"
}, 12), "asdad", "asdadad", DateTime.Now, true, 12)));



listOfQuery.Add("MultipleUpdate", await query.BulkUpdateCommandAsync(new[]
{
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12)
}));

listOfQuery.Add("SingleDelete", await query.DeleteCommandAsync(new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
{
    Id = 1,
    Name = "test"
}, 12), "asdad", "asdadad", DateTime.Now, true, 12)));

listOfQuery.Add("MultipleDelete", await query.BulkDeleteCommandAsync(new[]
{
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12),
    new User(1, new Person(1, "asdad", "adasd", 123, GenderType.Male, new Status
    {
        Id = 1,
        Name = "test"
    }, 12), "asdad", "asdadad", DateTime.Now, true, 12)
}));

Console.Write("Final-test");


// Snip : Select Query sample on multi entities with main entity result 




// Snip : Select Query sample on multi entities with multi entities results



// Snip : Select Query sample on multi entities with AggregateWindowFunction columns



// Snip : Select Query sample on multi entities with function columns


#endregion

