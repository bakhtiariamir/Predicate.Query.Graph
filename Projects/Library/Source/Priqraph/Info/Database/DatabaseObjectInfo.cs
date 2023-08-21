using Priqraph.Contract;
using Priqraph.DataType;
using System;

namespace Priqraph.Info.Database;

public class DatabaseObjectInfo : ObjectInfo<IColumnPropertyInfo>, IDatabaseObjectInfo
{
    public string DataSet
    {
        get;
        init;
    }

    public string Schema
    {
        get;
        init;
    }

    public DataSetType DataSetType
    {
        get;
    }

    public override ObjectInfoType Type => ObjectInfoType.Database;

    public DatabaseObjectInfo(string dataSet, DataSetType dataSetType, Type objectType, IEnumerable<IColumnPropertyInfo> propertyInfos, string schema = "dbo") : base(propertyInfos, ObjectInfoType.Database, objectType)
    {
        DataSet = dataSet;
        DataSetType = dataSetType;
        Schema = schema;
    }

    public override string ToString() => $"[{Schema}].[{DataSet}]";

    public static new IDatabaseObjectInfo CastObject(object value)
    {
        if (value is IDatabaseObjectInfo info)
            return info;

        try
        {
            return (IDatabaseObjectInfo)Convert.ChangeType(value, typeof(IDatabaseObjectInfo));
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException(); //todo
        }
    }
}
