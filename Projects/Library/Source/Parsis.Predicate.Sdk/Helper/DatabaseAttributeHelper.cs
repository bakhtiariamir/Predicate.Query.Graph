using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Info.Database;
using Parsis.Predicate.Sdk.Info.Database.Attribute;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseAttributeHelper
{

    public static IDatabaseObjectInfo<TObject> GetObjectInfo<TObject>(this Type type) where TObject : class
    {
        var dataSetInfo = type.GetClassAttribute<DataSetInfoAttribute>();
        string dataSet = "";
        DataSetType dataSetType = DataSetType.Function;
        string schema = "";
        if (dataSetInfo != null)
        {
            dataSet = dataSetInfo.DataSetName;
            dataSetType = dataSetInfo.Type;
            schema = dataSetInfo.SchemaName;
        }


        var properties = new List<IColumnPropertyInfo>();
        type.GetProperties().ToList().ForEach(property =>
        {
            var info = property.GetPropertyAttribute<ColumnInfoAttribute>();
            if (info != null)
            {
                //ToDo : This field value should be generated
                //toDo : Add PK Attribute and Identity with seed option
                var fkAlias = "";
                bool required = info.Required ?? false;
                RelationType relationType = RelationType.Optional;
                properties.Add(new ColumnPropertyInfo(info.Name, info.IsPrimaryKey, info.DataType, info.Type, info.FunctionName, fkAlias, info.AggregationFunctionType, relationType, required, info.Title, info.Alias, info.ErrorMessage));
            }
        });

        return new DatabaseObjectInfo<TObject>(dataSet, dataSetType, properties, schema);
    }

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo<TObject>(this Type type) where TObject : class => type.GetClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? GetClassAttribute<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetClassAttribute<TAttribute>(this Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : class => ((MemberExpression)propertyExpression.Body).Member.GetPropertyAttribute(valueSelector);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);


    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);


    public static TAttribute? GetPropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? GetPropertyAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

}
