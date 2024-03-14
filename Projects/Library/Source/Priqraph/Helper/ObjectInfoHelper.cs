using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Info;
using System.ComponentModel.DataAnnotations;

namespace Priqraph.Helper
{
    public static class ObjectInfoHelper
    {
        public static IObjectInfo<IPropertyInfo>? LastObjectInfo(this ICacheInfoCollection cacheInfoCollection, Type type)
        {
            if (!cacheInfoCollection.TryGetFirst(type.Name, out var objectInfo))
            {
                var cacheKey = $"cache_general:{type.Name}";
                objectInfo = type.ObjectInfo();
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
            objectInfo = cacheInfoCollection.LastObjectInfo(type);
            return objectInfo != null;
        }

        public static IObjectInfo<IPropertyInfo>? LastObjectInfo<TObject>(this ICacheInfoCollection cacheInfoCollection) where TObject : IQueryableObject
        {
            var type = typeof(TObject);
            if (!cacheInfoCollection.TryGetFirst(type.Name, out var objectInfo))
            {
                var cacheKey = $"cache_general:{type.Name}";
                objectInfo = type.ObjectInfo();
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
            objectInfo = cacheInfoCollection.LastObjectInfo<TObject>();
            return objectInfo != null;
        }

        public static IObjectInfo<IPropertyInfo> ObjectInfo(this Type type)
        {
            var properties = new List<IPropertyInfo>();
            type.GetProperties().ToList().ForEach(property =>
            {
                PropertyInfo(property, properties);
            });

            type.GetInterfaces()?.ToList().ForEach(inter =>
            {
                inter.GetProperties().ToList().ForEach(property =>
                {
                    PropertyInfo(property, properties);
                });
            });

            return new ObjectInfo<IPropertyInfo>(properties, ObjectInfoType.Unknown, type);
        }

        public static IEnumerable<IPropertyInfo> ObjectPropertiesInfo(this ICacheInfoCollection cacheInfoCollection, Type type)
        {
            var objectInfo = cacheInfoCollection.LastObjectInfo(type) ?? throw new ArgumentNullException($"Object information is null for {type.Name}.");
            return objectInfo.PropertyInfos;
        }

        public static IEnumerable<IPropertyInfo> ObjectPropertiesInfo(this ICacheInfoCollection cacheInfoCollection, Type type, Func<IPropertyInfo, bool> predicate)
        {
            var objectInfo = cacheInfoCollection.LastObjectInfo(type) ?? throw new ArgumentNullException($"Object information is null for {type.Name}.");
            return objectInfo.PropertyInfos.Where(predicate);
        }

        public static IEnumerable<IPropertyInfo> ObjectPropertiesInfo<TObject>(this ICacheInfoCollection cacheInfoCollection) => cacheInfoCollection.ObjectPropertiesInfo(typeof(TObject));

        public static IEnumerable<IPropertyInfo> ObjectPropertiesInfo<TObject>(this ICacheInfoCollection cacheInfoCollection, Func<IPropertyInfo, bool> predicate) => cacheInfoCollection.ObjectPropertiesInfo(typeof(TObject), predicate);


        public static string KeyName(Type type, ICacheInfoCollection infoCollection)
        {
            var objectInfo = infoCollection.LastObjectInfo(type) ?? throw new ArgumentNullException(nameof(type), $"Object Info cannot be null for {type.Name}.");
            var key = objectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new ArgumentNullException(nameof(IPropertyInfo.Key), $"key not found for {type.Name}.");
            return key.Name;
        }

        private static void PropertyInfo(System.Reflection.PropertyInfo property, List<IPropertyInfo> properties)
        {
            var isObject = property.PropertyType.GetInterface(nameof(IQueryableObject)) != null;

            var info = property.PropertyAttribute<BasePropertyAttribute>();
            if (info != null)
            {
                properties.Add(new Info.PropertyInfo(info.Key, info.Name, info.IsUnique, info.ReadOnly, info.NotMapped, info.DataType, property.PropertyType, info.Required, info.Title, info.DefaultValue, isObject, info.MaxLength == 0 ? null : info.MaxLength, info.MinLength == 0 ? null : info.MinLength, info.UniqueFieldGroup, info.RegexValidator, info.RegexError));
            }
            else
            {
                var required = property.PropertyAttribute<RequiredAttribute>() != null || property.PropertyType.IsNullable();
                var columnDataType = property.PropertyType.ColumnDataType();
                properties.Add(new Info.PropertyInfo(property.Name == "Id", property.Name, false, false, false, columnDataType, property.PropertyType, required, property.Name, null, isObject));
            }
        }
    }
} 
