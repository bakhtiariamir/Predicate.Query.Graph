using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Info.Database.Attribute;

namespace Parsis.Predicate.Console.Model;

[DataSetInfo("User", DataSetType.Table, "Membership")]
public class User : IQueryableObject
{
    public User(int id, Person person, string username, string password, DateTime creationDate, bool isActive, int count)
    {
        Id = id;
        Person = person;
        Username = username;
        Password = password;
        CreationDate = creationDate;
        IsActive = isActive;
        Count = count;
    }

    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, isPrimaryKey: true, required: true)]
    public int Id
    {
        get;
        set;
    }

    [ColumnInfo("PersonId", ColumnDataType.Int, DatabaseFieldType.Column, "Person", required: true)]
    public Person Person
    {
        get;
        set;
    }

    [ColumnInfo("Username", ColumnDataType.String, DatabaseFieldType.Column, required: true)]
    public string Username
    {
        get;
        set;
    }

    [ColumnInfo("Password", ColumnDataType.String, DatabaseFieldType.Column, required: true)]
    public string Password
    {
        get;
        set;
    }

    [ColumnInfo("CreationDate", ColumnDataType.DateTime, DatabaseFieldType.Column, required: true)]
    public DateTime CreationDate
    {
        get;
        set;
    }

    [ColumnInfo("IsActive", ColumnDataType.Boolean, DatabaseFieldType.Column, required: true)]
    public bool IsActive
    {
        get;
        set;
    }
    [ColumnInfo("Count", ColumnDataType.Int, DatabaseFieldType.Aggregation, "Count", false, false, AggregationFunctionType.Count)]
    public int Count
    {
        get;
        set;
    }



}
