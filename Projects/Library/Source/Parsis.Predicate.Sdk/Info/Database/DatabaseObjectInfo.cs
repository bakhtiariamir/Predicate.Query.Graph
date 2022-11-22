using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;

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

    public override Type ObjectType
    {
        get;
    }

    public override IEnumerable<IColumnPropertyInfo> PropertyInfos
    {
        get;
    }
    public override ObjectInfoType Type => ObjectInfoType.Database;

    public DatabaseObjectInfo(string dataSet, DataSetType dataSetType, Type objectType, IEnumerable<IColumnPropertyInfo> propertyInfos, string schema = "dbo")
    {
        DataSet = dataSet;
        DataSetType = dataSetType;
        ObjectType = objectType;
        PropertyInfos = propertyInfos;
        Schema = schema;
    }

    public override string ToString() => $"[{Schema}.{DataSet}]";
}