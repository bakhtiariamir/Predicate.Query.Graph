using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Info.Database;
using Parsis.Predicate.Sdk.Info.Database.Attribute;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseObjectInfoHelper
{
    public static IDatabaseObjectInfo? GetLastObjectInfo<TObject>(this IDatabaseCacheInfoCollection databaseCacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        if (!databaseCacheInfoCollection.TryGet(databaseCacheInfoCollection.GetKey(type.Name), out IDatabaseObjectInfo? objectInfo))
        {
            objectInfo = type.GetObjectInfo();
            databaseCacheInfoCollection.InitCache(type.Name, objectInfo);
        }

        return objectInfo;
    }

    public static IDatabaseObjectInfo GetObjectInfo(this Type type)
    {
        var dataSetInfo = type.GetClassAttribute<DataSetInfoAttribute>();
        var dataSet = "";
        var dataSetType = DataSetType.Function;
        var schema = "";
        if (dataSetInfo != null)
        {
            dataSet = dataSetInfo.DataSetName;
            dataSetType = dataSetInfo.Type;
            schema = dataSetInfo.SchemaName;
        }
        else
        {
//Get From 
//if null set default
        }

        var properties = new List<IColumnPropertyInfo>();
        type.GetProperties().ToList().ForEach(property =>
        {
            var info = property.GetPropertyAttribute<ColumnInfoAttribute>();
            if (info != null)
            {
                var required = info.Required ?? false;
                if (info.RankingFunctionType != RankingFunctionType.None && info.AggregateFunctionType != AggregateFunctionType.None) throw new NotSupported(dataSet, info.Name, ExceptionCode.DataSetInfoAttribute, "Property can not become aggregateWindowFunction and rankingWindowFunction");

                if (!info.NotMapped)
                    properties.Add(new ColumnPropertyInfo(schema, dataSet, info.ColumnName, info.Name, info.IsPrimaryKey, info.IsIdentity, info.DataType, info.Type, info.ReadOnly, info.NotMapped, info.FunctionName, info.AggregateFunctionType, info.RankingFunctionType, required, info.Title, info.ErrorMessage));
            }
            else
            {
                //ToDo : Set default config for columns
            }
        });

        return new DatabaseObjectInfo(dataSet, dataSetType, type, properties, schema);
    }

    public static IEnumerable<IColumnPropertyInfo> GetProperties(this IEnumerable<IColumnPropertyInfo> columnPropertyInfos, IColumnPropertyInfo parent, bool setParent = true)
    {
        foreach (var column in columnPropertyInfos)
        {
            if (setParent) column.SetRelationalObject(parent);
            yield return column;
        }
    }

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo<TObject>(this Type type) where TObject : IQueryableObject => type.GetClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? GetClassAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetClassAttribute<TAttribute>(this Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : IQueryableObject => ((MemberExpression)propertyExpression.Body).Member.GetPropertyAttribute(valueSelector);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetPropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? GetPropertyAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
}
