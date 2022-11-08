using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;

namespace Parsis.Predicate.Sdk.Info;

public class SqlServerObjectInfo<TObject> : DatabaseObjectInfo<TObject>, ISqlServerObjectInfo<TObject> where TObject : class
{
    public override IEnumerable<IPropertyInfo> PropertyInfos
    {
        get;
    }



    public override ObjectInfoType Type => ObjectInfoType.DatabaseSqlServer;
    public SqlServerObjectInfo(string table, string schema, IEnumerable<IPropertyInfo> propertyInfos)
    {
        PropertyInfos = propertyInfos;
        base.Table = table;
        base.Schema = schema;
    }
}