using Dynamitey;
using Priqraph.Contract;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Helper;
using System.Linq.Expressions;

namespace Priqraph.Generator.Cache.MemoryCache;

public class CommandVisitor : CacheVisitor<CacheCommandQueryPart>
{
    public CommandVisitor(ICacheInfoCollection cacheObjectCollection, IObjectInfo<IPropertyInfo> objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override CacheCommandQueryPart VisitMember(MemberExpression expression)
    {
        if (expression.Expression == null)
            throw new NotSupportedOperationException(ExceptionCode.ObjectInfo); //ToDo

        var valueQueryPart = Visit(expression.Expression);

        return valueQueryPart;


        //var columnValue = valueQueryPart.Parameter.CachePredicates?.FirstOrDefault() ?? throw new NotSupported(ExceptionCode.ApiQueryBuilder); //todo

        //if (columnValue.Records is not null)
        //{
        //    var objectValue = columnValue.Records.FirstOrDefault();

        //    var member = expression.Member;
        //    var value = member switch
        //    {
        //        FieldInfo field => field.GetValue(objectValue),
        //        PropertySelector info => info.GetValue(objectValue, null),
        //        _ => throw new NotSupported("asd")
        //    } ?? throw new NotFound("asd");

        //    if (value.GetType().IsArray)
        //        return CacheCommandQueryPart.Merge(null ,CacheCommandQueryPart.Create(new ColumnPropertyCollection(value as IEnumerable<object>)));

        //    foreach (var column in columnProperties)
        //    {
        //        var dynamicValue = Dynamic.InvokeGet(value, column.ColumnPropertyInfo?.Name) ?? column.ColumnPropertyInfo?.DefaultValue;
        //        GetOption("Command", out var command);
        //        if (command.ToString() != "Edit" && (column.ColumnPropertyInfo?.Required ?? false) && dynamicValue is null && column.ColumnPropertyInfo.DefaultValue is null)
        //            throw new ArgumentNullException($"Value of {column.ColumnPropertyInfo.Name} can not be null.");

        //        if (dynamicValue == null)
        //        {
        //            column.SetValue(null);
        //            continue;
        //        }

        //        if (column.ColumnPropertyInfo?.Type.GetInterface(nameof(IQueryableObject)) != null)
        //        {
        //            var cacheObject = CacheObjectCollection.GetLastDatabaseObjectInfo(column.ColumnPropertyInfo?.Type!) ?? throw new System.Exception(); //todo
        //            var key = cacheObject?.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new System.Exception(); //todo

        //            var objectColumnValue = Dynamic.InvokeGet(dynamicValue, key.Name);
        //            column.SetValue(objectColumnValue);
        //        }
        //        else
        //        {
        //            column.SetValue(dynamicValue);
        //        }
        //    }

        //    return CacheCommandQueryPart.Create(columnProperties);
        //}

        //throw new System.Exception(); //todo
    }

    protected override CacheCommandQueryPart VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
    {
        var valueObject = expression.ObjectValue() ?? throw new NotSupportedOperationException("easd"); //todo
        var baseQueryParameter = new List<CacheClausePredicate>();
        var keyProperty = ObjectInfo.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new System.Exception(); //todo
        if (valueObject.GetType().IsArray)
        {
            foreach (var recordValue in (IEnumerable<object>)valueObject)
            {
                var keyValue = Dynamic.InvokeGet(recordValue, keyProperty.Name);
                baseQueryParameter.Add(new CacheClausePredicate(keyValue, recordValue));
            }
        }
        else
        {
            var keyValue = Dynamic.InvokeGet(valueObject, keyProperty.Name);
            baseQueryParameter.Add(new CacheClausePredicate(keyValue, valueObject));
        }

        return CacheCommandQueryPart.Create(baseQueryParameter);
    }
}

