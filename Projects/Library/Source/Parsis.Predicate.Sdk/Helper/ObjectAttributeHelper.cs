using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Parsis.Predicate.Sdk.Helper;
public static class ObjectAttributeHelper
{
    public static IDictionary<string, string> FieldAttribute<TObject>(this TObject type) where TObject : class
    {
        var fields = new Dictionary<string, string>();



        return fields;
    }






    public static string TableName<TObject>(this TObject type) where TObject : class => type.GetClassAttribute<TObject, TableAttribute, string>(item => item.Name) ?? string.Empty;

    public static string TableSchemaName<TObject>(this TObject type) where TObject : class => type.GetClassAttribute<TObject, TableAttribute, string>(item => item.Schema) ?? string.Empty;

    private static TValue? GetClassAttribute<TObject, TAttribute, TValue>(this TObject type, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        where TObject : class
    {
        return type.GetType().GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);
    }

    private static TValue? GetPropertyAttribute<TObject, TAttribute, TValue>(this TObject type, Expression<Func<TObject, object>> propertyExpression, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        where TObject : class
    {
        var property = ((MemberExpression)propertyExpression.Body).Member;
        return property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default(TValue);

    }
}
