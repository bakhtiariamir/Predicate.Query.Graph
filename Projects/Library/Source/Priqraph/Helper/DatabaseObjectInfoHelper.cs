using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.Info.Database;
using Priqraph.Info.Database.Attribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Priqraph.Helper;

public static class DatabaseObjectInfoHelper
{
    public static IDatabaseObjectInfo? LastDatabaseObjectInfo(this ICacheInfoCollection databaseCacheInfoCollection, Type type)
    {
        var cacheKey = $"cache_database:{type.Name}";
        if (!databaseCacheInfoCollection.TryGet(cacheKey, out var objectInfo))
        {
            objectInfo = type.DatabaseObjectInfo();
            databaseCacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        if (objectInfo != null)
            return Info.Database.DatabaseObjectInfo.CastObject(objectInfo);

        return default;
    }

    public static bool TryGetLastDatabaseObjectInfo(this ICacheInfoCollection cacheInfoCollection, Type type, out IDatabaseObjectInfo? objectInfo)
    {
        objectInfo = cacheInfoCollection.LastDatabaseObjectInfo(type);
        return objectInfo != null;
    }

    public static IDatabaseObjectInfo? LastDatabaseObjectInfo<TObject>(this ICacheInfoCollection databaseCacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        var cacheKey = $"cache_database:{type.Name}";
        if (!databaseCacheInfoCollection.TryGet(cacheKey, out var objectInfo))
        {
            objectInfo = type.DatabaseObjectInfo();
            databaseCacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        if (objectInfo != null)
            return Info.Database.DatabaseObjectInfo.CastObject(objectInfo);

        return default;
    }

    private static IDatabaseObjectInfo DatabaseObjectInfo(this Type type)
    {
        var dataSetInfo = type.ClassAttribute<DataSetInfoAttribute>();
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
            var dataAnnotationInfo = type.ClassAttribute<TableAttribute>();
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
            property.ColumnPropertyInfo(dataSet, properties, schema);
        });

        type.GetInterfaces()?.ToList().ForEach(inter =>
        {
            inter.GetProperties().ToList().ForEach(property =>
            {
                property.ColumnPropertyInfo(dataSet, properties, schema);
            });
        });

        return new DatabaseObjectInfo(dataSet, dataSetType, type, properties, schema);
    }

    private static void ColumnPropertyInfo(this System.Reflection.PropertyInfo property, string dataSet, List<IColumnPropertyInfo> properties, string schema)
    {
        var isObject = property.PropertyType.GetInterface(nameof(IQueryableObject)) != null;

        var info = property.PropertyAttribute<ColumnInfoAttribute>();
        if (info != null)
        {
            if (info.RankingFunctionType != RankingFunctionType.None && info.AggregateFunctionType != AggregateFunctionType.None) throw new NotSupportedOperationException(dataSet, info.Name, ExceptionCode.DataSetInfoAttribute, "Property can not become aggregateWindowFunction and rankingWindowFunction");

            if (!info.NotMapped)
            {
                properties.Add(new ColumnPropertyInfo(schema, dataSet, info.ColumnName, info.Name, info.Key, info.Identity, info.DataType, info.Type, property.PropertyType, info.IsUnique, info.ReadOnly, info.NotMapped, info.FunctionName, info.AggregateFunctionType, info.RankingFunctionType, info.Required, info.Title,  info.WindowPartitionColumns, info.WindowOrderColumns, info.DefaultValue, isObject, info.MaxLength, info.MinLength, info.UniqueFieldGroup, info.RegexValidator, info.RegexError, isLabel: info.IsLabel));
            }
        }
        else
        {
            var required = property.PropertyAttribute<RequiredAttribute>() != null || property.PropertyType.IsNullable();
            var columnDataType = property.PropertyType.ColumnDataType();
            var notMapped = property.PropertyAttribute<NotMappedAttribute>() != null;
            var isPrimaryKey = property.PropertyAttribute<KeyAttribute>() != null || property.Name == "Id";
            var readOnly = property.PropertyAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? property.CanWrite;
            if (!notMapped)
                properties.Add(new ColumnPropertyInfo(schema, dataSet, property.Name, property.Name, isPrimaryKey, isPrimaryKey, columnDataType, DatabaseFieldType.Column, property.PropertyType, false, readOnly, notMapped, required: required, isObject: isObject));
        }
    }

    public static IEnumerable<IColumnPropertyInfo> Properties(this IEnumerable<IColumnPropertyInfo> columnPropertyInfos, IColumnPropertyInfo parent, bool setParent = true)
    {
        foreach (var propertyInfo in columnPropertyInfos)
        {
            var property = propertyInfo.Clone();
            if (!property.NotMapped && setParent) property.SetRelationalObject(parent);
            yield return property;
        }
    }

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo(this Type type) => type.ClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? ClassAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TAttribute? ClassAttribute<TAttribute>(this Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? PropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : IQueryableObject => ((MemberExpression)propertyExpression.Body).Member.PropertyAttribute(valueSelector);

    public static TValue? PropertyAttribute<TAttribute, TValue>(this System.Reflection.PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TValue? PropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TAttribute? PropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? PropertyAttribute<TAttribute>(this System.Reflection.PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static bool IsNullable(this Type type)
    {
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
    }
}
