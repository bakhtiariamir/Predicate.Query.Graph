using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Info.Database.Attribute;

namespace Priqraph.Console.Model;

[DataSetInfo("Person", DataSetType.Table, "Membership")]
public class Person : IQueryableObject
{
    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, "Id", true, true, true, true)]
    public int Id
    {
        get;
        set;
    }

    [ColumnInfo("Name", ColumnDataType.String, DatabaseFieldType.Column, required: true, defaultValue: "Ali")]
    public string? Name
    {
        get;
        set;
    }

    [ColumnInfo("LastName", ColumnDataType.String, DatabaseFieldType.Column, required: true)]
    public string Family
    {
        get;
        set;
    }

    [ColumnInfo("Age", ColumnDataType.Int, DatabaseFieldType.Column)]
    public int Age
    {
        get;
        set;
    }

    [ColumnInfo("Gender", ColumnDataType.Byte, DatabaseFieldType.Column, "GenderType", required: true)]
    public GenderType GenderType
    {
        get;
        set;
    }

    [ColumnInfo("StatusId", ColumnDataType.Int, DatabaseFieldType.Column, "Status", required: false)]
    public Status Status
    {
        get;
        set;
    }

    [ColumnInfo("Age", ColumnDataType.Int, DatabaseFieldType.AggregateWindowFunction, "Sum", false, required: false,  aggregateFunctionType: AggregateFunctionType.Sum)]
    public int Sum
    {
        get;
        set;
    }


    public Person(int id, string? name, string family, int age, GenderType genderType, Status status, int sum)
    {
        Id = id;
        Name = name;
        Age = age;
        GenderType = genderType;
        Status = status;
        Sum = sum;
        Family = family;
    }
}

[DataSetInfo("Status", DataSetType.Table, "Core")]
public class Status : IQueryableObject
{
    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, "Id", true, true, true, true)]
    public int Id
    {
        get;
        set;
    }

    [ColumnInfo("Name", ColumnDataType.String, DatabaseFieldType.Column, required: true)]
    public string Name
    {
        get;
        set;
    }
}

public enum GenderType
{
    Male,
    Female
}