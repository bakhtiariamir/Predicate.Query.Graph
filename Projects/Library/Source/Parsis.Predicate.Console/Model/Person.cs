using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Info.Database.Attribute;

namespace Parsis.Predicate.Console.Model;

[DataSetInfo("Person", DataSetType.Table, "Membership")]
public class Person
{
    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, isPrimaryKey:true, required:true)]
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

    [ColumnInfo("Gender", ColumnDataType.Byte, DatabaseFieldType.Column, "GenderType" , required: true)]
    public GenderType GenderType
    {
        get;
        set;
    }

    [ColumnInfo("StatusId", ColumnDataType.Int, DatabaseFieldType.Column, "Status", required: true)]
    public Status Status
    {
        get;
        set;
    }


    public Person(int id, string name, string family, int age, GenderType genderType, Status status)
    {
        Id = id;
        Name = name;
        Age = age;
        GenderType = genderType;
        Status = status;
        Family = family;
    }
}

[DataSetInfo("Status", DataSetType.Table, "Core")]
public class Status
{
    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, isPrimaryKey: true, required: true)]
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