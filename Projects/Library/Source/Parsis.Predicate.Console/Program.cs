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
//var select = await query.SelectQueryAsync();

var query = new UserDatabaseQuery(databaseCacheInfoCollection);
var select = await query.SelectQueryAsync();


// Snip : Select Query sample on multi entities with main entity result 




// Snip : Select Query sample on multi entities with multi entities results



// Snip : Select Query sample on multi entities with Aggregation columns



// Snip : Select Query sample on multi entities with function columns


#endregion

