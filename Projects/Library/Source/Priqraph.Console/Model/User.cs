﻿using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Info.Database.Attribute;

namespace Priqraph.Console.Model;

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

    [ColumnInfo("Id", ColumnDataType.Int, DatabaseFieldType.Column, isPrimaryKey: true, isIdentity:true, required: true)]
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

    [ColumnInfo("Username", ColumnDataType.String, DatabaseFieldType.Column, required: true, defaultValue:"AliAlie")]
    public string? Username
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

    [ColumnInfo("IsActive", ColumnDataType.Boolean, DatabaseFieldType.Column, readOnly:true)]
    public bool IsActive
    {
        get;
        set;
    }
    [ColumnInfo("Count", ColumnDataType.Int, DatabaseFieldType.AggregateWindowFunction, "Count", false, required: false, aggregateFunctionType: AggregateFunctionType.Count)]
    public int Count
    {
        get;
        set;
    }
}
