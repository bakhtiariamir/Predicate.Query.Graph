using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.Info;
using Parsis.Predicate.Sdk.Info.Database;
using Parsis.Predicate.Sdk.Info.Database.Attribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Helper;

public static class ObjectInfoHelper
{
    public static IObjectInfo<IPropertyInfo>? GetLastObjectInfo(this ICacheInfoCollection cacheInfoCollection, Type type)
    {
        if (!cacheInfoCollection.TryGetFirst(type.Name, out var objectInfo))
        {
            var cacheKey = $"cache_general:{type.Name}";
            objectInfo = type.GetObjectInfo();
            cacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        //خطا باید درست شود
        //بعد هر جا از جمله Domain.Setter قبل از Repository از ObjectInfo استفاده کند.
        //چک شود جایی DatabaseObjectIfno بود درست شود.
        if (objectInfo != null)
            return ObjectInfo<IPropertyInfo>.CastObject(objectInfo);

        return default;
    }

    public static bool TryGetLastObjectInfo(this ICacheInfoCollection cacheInfoCollection, Type type, out IObjectInfo<IPropertyInfo>? objectInfo)
    {
        objectInfo = cacheInfoCollection.GetLastObjectInfo(type);
        return objectInfo != null;
    }

    public static IObjectInfo<IPropertyInfo>? GetLastObjectInfo<TObject>(this ICacheInfoCollection cacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        if (!cacheInfoCollection.TryGetFirst(type.Name, out var objectInfo))
        {
            var cacheKey = $"cache_general:{type.Name}";
            objectInfo = type.GetObjectInfo();
            cacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        //خطا باید درست شود
        //بعد هر جا از جمله Domain.Setter قبل از Repository از ObjectInfo استفاده کند.
        //چک شود جایی DatabaseObjectIfno بود درست شود.
        if (objectInfo != null)
            return ObjectInfo<IPropertyInfo>.CastObject(objectInfo);

        return default;
    }

    public static bool TryGetLastObjectInfo<TObject>(this ICacheInfoCollection cacheInfoCollection, out IObjectInfo<IPropertyInfo>? objectInfo) where TObject : IQueryableObject
    {
        objectInfo = cacheInfoCollection.GetLastObjectInfo<TObject>();
        return objectInfo != null;
    }

    public static IObjectInfo<IPropertyInfo> GetObjectInfo(this Type type)
    {
        var properties = new List<IPropertyInfo>();
        type.GetProperties().ToList().ForEach(property =>
        {
            GetPropertyInfo(property, properties);
        });

        type.GetInterfaces()?.ToList().ForEach(inter =>
        {
            inter.GetProperties().ToList().ForEach(property =>
            {
                GetPropertyInfo(property, properties);
            });
        });

        return new ObjectInfo<IPropertyInfo>(properties, ObjectInfoType.Unknown, type);
    }

    public static IEnumerable<IPropertyInfo> GetObjectPropertiesInfo(this ICacheInfoCollection cacheInfoCollection, Type type)
    {
        var objectInfo = cacheInfoCollection.GetLastObjectInfo(type) ?? throw new ArgumentNullException($"Object information is null for {type.Name}.");
        return objectInfo.PropertyInfos;
    }

    public static IEnumerable<IPropertyInfo> GetObjectPropertiesInfo(this ICacheInfoCollection cacheInfoCollection, Type type, Func<IPropertyInfo, bool> predicate)
    {
        var objectInfo = cacheInfoCollection.GetLastObjectInfo(type) ?? throw new ArgumentNullException($"Object information is null for {type.Name}.");
        return objectInfo.PropertyInfos.Where(predicate);
    }

    public static IEnumerable<IPropertyInfo> GetObjectPropertiesInfo<TObject>(this ICacheInfoCollection cacheInfoCollection) => cacheInfoCollection.GetObjectPropertiesInfo(typeof(TObject));

    public static IEnumerable<IPropertyInfo> GetObjectPropertiesInfo<TObject>(this ICacheInfoCollection cacheInfoCollection, Func<IPropertyInfo, bool> predicate) => cacheInfoCollection.GetObjectPropertiesInfo(typeof(TObject), predicate);


    public static string GetKeyName(Type type, ICacheInfoCollection infoCollection)
    {
        var objectInfo = infoCollection.GetLastObjectInfo(type) ?? throw new ArgumentNullException(nameof(type), $"Objectinfo cannot be null for {type.Name}.");
        var key = objectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException(nameof(IPropertyInfo.Key), $"key not found for {type.Name}.");
        return key.Name;
    }

    private static void GetPropertyInfo(System.Reflection.PropertyInfo property, List<IPropertyInfo> properties)
    {
        var isObject = property.PropertyType.GetInterface(nameof(IQueryableObject)) != null;

        var info = property.GetPropertyAttribute<BasePropertyAttribute>();
        if (info != null)
        {
            properties.Add(new Info.PropertyInfo(info.Key, info.Name, info.IsUnique, info.ReadOnly, info.NotMapped, info.DataType, property.PropertyType, info.Required, info.Title, info.ErrorMessage, info.DefaultValue, isObject, info.MaxLength == 0 ? null : info.MaxLength, info.MinLength == 0 ? null : info.MinLength, info.UniqueFieldGroup));
        }
        else
        {
            var required = property.GetPropertyAttribute<RequiredAttribute>() != null || property.PropertyType.IsNullable();
            var columnDataType = property.PropertyType.GetColumnDataType();
            properties.Add(new Info.PropertyInfo(property.Name =="Id" , property.Name, false, false, false, columnDataType, property.PropertyType, required, property.Name, null,   null, isObject));
        }
    }
}

public static class DatabaseObjectInfoHelper
{
    public static IDatabaseObjectInfo? GetLastDatabaseObjectInfo(this ICacheInfoCollection databaseCacheInfoCollection, Type type)
    {
        var cacheKey = $"cache_database:{type.Name}";
        if (!databaseCacheInfoCollection.TryGet(cacheKey, out var objectInfo))
        {
            objectInfo = type.GetDatabaseObjectInfo();
            databaseCacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        if (objectInfo != null)
            return DatabaseObjectInfo.CastObject(objectInfo);

        return default;
    }

    public static bool TryGetLastDatabaseObjectInfo(this ICacheInfoCollection cacheInfoCollection, Type type, out IDatabaseObjectInfo? objectInfo)
    {
        objectInfo = cacheInfoCollection.GetLastDatabaseObjectInfo(type);
        return objectInfo != null;
    }

    public static IDatabaseObjectInfo? GetLastDatabaseObjectInfo<TObject>(this ICacheInfoCollection databaseCacheInfoCollection) where TObject : IQueryableObject
    {
        var type = typeof(TObject);
        var cacheKey = $"cache_database:{type.Name}";
        if (!databaseCacheInfoCollection.TryGet(cacheKey, out var objectInfo))
        {
            objectInfo = type.GetDatabaseObjectInfo();
            databaseCacheInfoCollection.InitCache(cacheKey, objectInfo);
        }

        if (objectInfo != null)
            return DatabaseObjectInfo.CastObject(objectInfo);

        return default;
    }

    public static bool TryGetLastDatabaseObjectInfo<TObject>(this ICacheInfoCollection cacheInfoCollection, out IDatabaseObjectInfo? objectInfo) where TObject : IQueryableObject
    {
        objectInfo = cacheInfoCollection.GetLastDatabaseObjectInfo<TObject>();
        return objectInfo != null;
    }

    public static IDatabaseObjectInfo GetDatabaseObjectInfo(this Type type)
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
            GetColumnPropertyInfo(property, dataSet, properties, schema);
        });

        type.GetInterfaces()?.ToList().ForEach(inter =>
        {
            inter.GetProperties().ToList().ForEach(property =>
            {
                GetColumnPropertyInfo(property, dataSet, properties, schema);
            });
        });

        return new DatabaseObjectInfo(dataSet, dataSetType, type, properties, schema);
    }

    private static void GetColumnPropertyInfo(System.Reflection.PropertyInfo property, string dataSet, List<IColumnPropertyInfo> properties, string schema)
    {
        var isObject = property.PropertyType.GetInterface(nameof(IQueryableObject)) != null;

        var info = property.GetPropertyAttribute<ColumnInfoAttribute>();
        if (info != null)
        {
            if (info.RankingFunctionType != RankingFunctionType.None && info.AggregateFunctionType != AggregateFunctionType.None) throw new NotSupported(dataSet, info.Name, ExceptionCode.DataSetInfoAttribute, "Property can not become aggregateWindowFunction and rankingWindowFunction");

            if (!info.NotMapped)
            {
                properties.Add(new ColumnPropertyInfo(schema, dataSet, info.ColumnName, info.Name, info.Key, info.Identity, info.DataType, info.Type, property.PropertyType, info.IsUnique, info.ReadOnly, info.NotMapped, info.FunctionName, info.AggregateFunctionType, info.RankingFunctionType, info.Required, info.Title, info.ErrorMessage, info.WindowPartitionColumns, info.WindowOrderColumns, info.DefaultValue, isObject, info.MaxLength, info.MinLength, info.UniqueFieldGroup));
            }
        }
        else
        {
            var required = property.GetPropertyAttribute<RequiredAttribute>() != null || property.PropertyType.IsNullable();
            var columnDataType = property.PropertyType.GetColumnDataType();
            var notMapped = property.GetPropertyAttribute<NotMappedAttribute>() != null;
            var isPrimaryKey = property.GetPropertyAttribute<KeyAttribute>() != null || property.Name == "Id";
            var readOnly = property.GetPropertyAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? property.CanWrite;
            if (!notMapped)
                properties.Add(new ColumnPropertyInfo(schema, dataSet, property.Name, property.Name, isPrimaryKey, isPrimaryKey, columnDataType, DatabaseFieldType.Column, property.PropertyType, false, readOnly, notMapped, required: required, isObject: isObject));
        }
    }

    public static IEnumerable<IColumnPropertyInfo> GetProperties(this IEnumerable<IColumnPropertyInfo> columnPropertyInfos, IColumnPropertyInfo parent, bool setParent = true)
    {
        foreach (var propertyInfo in columnPropertyInfos)
        {
            var property = propertyInfo.Clone();
            if (!property.NotMapped && setParent) property.SetRelationalObject(parent);
            yield return property;
        }
    }

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo(this Type type) => type.GetClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? GetClassAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TAttribute? GetClassAttribute<TAttribute>(this Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : IQueryableObject => ((MemberExpression)propertyExpression.Body).Member.GetPropertyAttribute(valueSelector);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this System.Reflection.PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;

    public static TAttribute? GetPropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? GetPropertyAttribute<TAttribute>(this System.Reflection.PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static bool IsNullable(this Type type)
    {
        if (!type.IsValueType) return true;
        if (Nullable.GetUnderlyingType(type) != null) return true;
        return false;
    }
}
