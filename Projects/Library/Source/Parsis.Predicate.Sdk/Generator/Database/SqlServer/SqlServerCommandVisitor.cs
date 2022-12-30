using Dynamitey;
using Parsis.Predicate.Sdk.Contract;
using Parsis.Predicate.Sdk.Exception;
using Parsis.Predicate.Sdk.ExpressionHandler.Visitors;
using Parsis.Predicate.Sdk.Helper;
using System.Linq.Expressions;
using System.Reflection;

namespace Parsis.Predicate.Sdk.Generator.Database.SqlServer;
public class SqlServerCommandVisitor : DatabaseVisitor<DatabaseCommandQueryPart>
{
    public SqlServerCommandVisitor(IDatabaseCacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override DatabaseCommandQueryPart VisitMember(MemberExpression expression)
    {
        if (expression.Expression == null)
            throw new NotSupported(ExceptionCode.ObjectInfo); //ToDo

        var columnProperties = ObjectInfo.PropertyInfos.Select(item => new ColumnProperty(item)).ToArray();
        var columnCommandQueryPart = DatabaseCommandQueryPart.Create(columnProperties);

        var valueQueryPart = Visit(expression.Expression);


        var columnValue = valueQueryPart.Parameter.ColumnPropertyCollections?.FirstOrDefault() ?? throw new NotFound("asd"); //todo

        if (columnValue.Records == null)
            throw new NotSupported(ExceptionCode.ApiQueryBuilder); //todo

        if (columnValue.Records is not null)
        {
            var objectValue = columnValue.Records.FirstOrDefault();

            var member = expression.Member;
            var value = member switch {
                FieldInfo field => field.GetValue(objectValue),
                PropertyInfo info => info.GetValue(objectValue, null),
                _ =>throw new NotSupported("asd")
            } ?? throw new NotFound("asd");

            if (value.GetType().IsArray)
                return DatabaseCommandQueryPart.Merge(null, columnCommandQueryPart, DatabaseCommandQueryPart.Create(new ColumnPropertyCollection(value as IEnumerable<object>)));

            foreach (var column in columnProperties)
            {
                var dynamicValue = Dynamic.InvokeGet(value, column.ColumnPropertyInfo?.Name);

                if ((column.ColumnPropertyInfo?.Required ?? false) && dynamicValue is null)
                    throw new NotSupported("e0"); //todo
                
                column.SetValue(dynamicValue);
            }

            return DatabaseCommandQueryPart.Create(columnProperties);
        }

        throw new System.Exception(); //todo
    }

    protected override DatabaseCommandQueryPart VisitConstant(ConstantExpression expression)
    {
        var value = expression.GetObject() ?? throw new NotSupported("easd"); //todo

        return DatabaseCommandQueryPart.Create(new ColumnPropertyCollection(new[]{ value }));
    }
}
