using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Info.Database;
using Parsis.Predicate.Sdk.Info.Database.Attribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Helper;

public static class DatabaseObjectInfoHelper
{
    public static IDatabaseObjectInfo? GetLastObjectInfo<TObject>(this IDatabaseCacheInfoCollection databaseCacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        if (!databaseCacheInfoCollection.TryGet(databaseCacheInfoCollection.GetKey(type.Name), out var objectInfo))
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
        var dataSetType = DataSetType.Table;
        var schema = "dbo";
        if (dataSetInfo != null)
        {
            dataSet = dataSetInfo.DataSetName;
            dataSetType = dataSetInfo.Type;
            schema = dataSetInfo.SchemaName;
        }
        else if (type.GetInterfaces().Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IQueryableObject<>)))
        {
            var instance = (IQueryableObject<IColumnPropertyInfo>)Activator.CreateInstance(type)!;
            return instance?.GetObjectInfo() as DatabaseObjectInfo ?? throw new InvalidOperationException(); // todo Exception
        }
        else
        {
            var dataAnnotationInfo = type.GetClassAttribute<TableAttribute>();
            if (dataAnnotationInfo != null)
            {
                dataSet = dataAnnotationInfo.Name;
                schema = dataAnnotationInfo.Schema ?? "dbo";
            }
            else
            {
                dataSet = nameof(type);
            }
        }

        var properties = new List<IColumnPropertyInfo>();
        type.GetProperties().ToList().ForEach(property =>
        {
            GetPropertyInfo(property, dataSet, properties, schema);
        });

        type.GetInterfaces()?.ToList().ForEach(inter =>
        {
            inter.GetProperties().ToList().ForEach(property =>
            {
                GetPropertyInfo(property, dataSet, properties, schema);
            });
        });

        return new DatabaseObjectInfo(dataSet, dataSetType, type, properties, schema);
    }

    private static void GetPropertyInfo(PropertyInfo property, string dataSet, List<IColumnPropertyInfo> properties, string schema)
    {
        var info = property.GetPropertyAttribute<ColumnInfoAttribute>();
        if (info != null)
        {
            var required = info.Required ?? false;
            if (info.RankingFunctionType != RankingFunctionType.None && info.AggregateFunctionType != AggregateFunctionType.None) throw new NotSupported(dataSet, info.Name, ExceptionCode.DataSetInfoAttribute, "Property can not become aggregateWindowFunction and rankingWindowFunction");

            if (!info.NotMapped)
                properties.Add(new ColumnPropertyInfo(schema, dataSet, info.ColumnName, info.Name, info.IsPrimaryKey, info.IsIdentity, info.DataType, info.Type, property.PropertyType, info.IsUnique, info.ReadOnly, info.NotMapped, info.FunctionName, info.AggregateFunctionType, info.RankingFunctionType, required, info.Title, null, info.ErrorMessage, info.WindowPartitionColumns, info.WindowOrderColumns, info.DefaultValue));
        }
        else
        {
            var required = property.GetPropertyAttribute<RequiredAttribute>() != null || property.PropertyType.IsNullable();
            var columnDataType = property.PropertyType.GetColumnDataType();
            var notMapped = property.GetPropertyAttribute<NotMappedAttribute>() != null;
            var isPrimaryKey = property.GetPropertyAttribute<KeyAttribute>() != null || property.Name == "Id";
            var readOnly = property.GetPropertyAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? property.CanWrite;
            if (!notMapped)
                properties.Add(new ColumnPropertyInfo(schema, dataSet, property.Name, property.Name, isPrimaryKey, true, columnDataType, DatabaseFieldType.Column, property.PropertyType, false, readOnly, notMapped, required: required));
        }
    }

    public static IEnumerable<IColumnPropertyInfo> GetProperties(this IEnumerable<IColumnPropertyInfo> columnPropertyInfos, IColumnPropertyInfo parent, bool setParent = true)
    {
        foreach (var column in columnPropertyInfos)
        {
            if (!column.NotMapped && setParent) column.SetRelationalObject(parent);
            yield return column;
        }
    }

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo<TObject>(this Type type) where TObject : IQueryableObject => type.GetClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? GetClassAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetClassAttribute<TAttribute>(this Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : IQueryableObject => ((MemberExpression)propertyExpression.Body).Member.GetPropertyAttribute(valueSelector);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetPropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? GetPropertyAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    private static bool IsNullable(this Type type)
    {
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
    }
}
