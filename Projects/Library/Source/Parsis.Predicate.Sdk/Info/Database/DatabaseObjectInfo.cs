using System;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info.Database;

public class DatabaseObjectInfo<TObject> : ObjectInfo<TObject>, IDatabaseObjectInfo<TObject> where TObject : class
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

    public override IEnumerable<IColumnPropertyInfo> PropertyInfos
    {
        get;
    }

    public override ObjectInfoType Type => ObjectInfoType.Database;

    public DatabaseObjectInfo(string dataSet, DataSetType dataSetType, IEnumerable<IColumnPropertyInfo> propertyInfos, string schema = "dbo")
    {
        DataSet = dataSet;
        DataSetType = dataSetType;
        PropertyInfos = propertyInfos;
        Schema = schema;
    }

}