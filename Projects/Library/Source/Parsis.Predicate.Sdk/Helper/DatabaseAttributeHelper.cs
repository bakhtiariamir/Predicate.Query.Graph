using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.DataType;
using Parsis.Predicate.Sdk.Info.Database;
using Parsis.Predicate.Sdk.Info.Database.Attribute;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Helper;
public static class DatabaseAttributeHelper
{

    public static IDatabaseObjectInfo GetObjectInfo(this System.Type type)
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
                bool required = info.Required ?? false;
                RelationType relationType = required ? RelationType.Required : RelationType.Optional;
                properties.Add(new ColumnPropertyInfo(schema, dataSet, info.ColumnName, info.Name, info.IsPrimaryKey, info.DataType, info.Type, info.FunctionName, info.AggregationFunctionType, relationType, required, info.Title, info.ErrorMessage));
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

    public static (string dataSetName, string schemaName, DataSetType dataSetType) DataSetInfo<TObject>(this System.Type type) where TObject : IQueryableObject => type.GetClassAttribute<DataSetInfoAttribute, (string dataSetName, string schemaName, DataSetType dataSetType)>(item => (item.DataSetName, item.SchemaName, item.Type));

    public static TValue? GetClassAttribute<TAttribute, TValue>(this System.Type type, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetClassAttribute<TAttribute>(this System.Type type) => (TAttribute?)type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : IQueryableObject => ((MemberExpression)propertyExpression.Body).Member.GetPropertyAttribute(valueSelector);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this PropertyInfo property, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TValue? GetPropertyAttribute<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute => member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    public static TAttribute? GetPropertyAttribute<TAttribute>(this MemberInfo member) where TAttribute : Attribute => (TAttribute?)member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();

    public static TAttribute? GetPropertyAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute => (TAttribute?)property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
}
