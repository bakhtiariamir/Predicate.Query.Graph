using Dynamitey;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;
using Priqraph.ExpressionHandler.Visitors;
using Priqraph.Helper;
using Priqraph.Query.Builders;
using System.Linq.Expressions;
using System.Reflection;

namespace Priqraph.Generator.Database.Visitors;

public class CommandVisitor : DatabaseVisitor<CommandQueryFragment>
{
    public CommandVisitor(ICacheInfoCollection cacheObjectCollection, IDatabaseObjectInfo objectInfo, ParameterExpression? parameterExpression) : base(cacheObjectCollection, objectInfo, parameterExpression)
    {
    }

    protected override CommandQueryFragment VisitMember(MemberExpression expression)
    {
        if (expression.Expression == null)
            throw new NotSupported(ExceptionCode.ObjectInfo); //ToDo

        var columnProperties = ObjectInfo.PropertyInfos.Select(item => new ColumnProperty(item)).Where(item => item.ColumnPropertyInfo?.FieldType != DatabaseFieldType.Related).ToArray();
        var columnCommandQueryPart = CommandQueryFragment.Create(columnProperties);

        var valueQueryPart = Visit(expression.Expression);

        var columnValue = valueQueryPart.Parameter?.ColumnPropertyCollections?.FirstOrDefault() ?? throw new NotFound("asd"); //todo

        if (columnValue.Records == null)
            throw new NotSupported(ExceptionCode.ApiQueryBuilder); //todo

        if (columnValue.Records is not null)
        {
            var objectValue = columnValue.Records.FirstOrDefault();

            var member = expression.Member;
            var value = member switch {
                FieldInfo field => field.GetValue(objectValue),
                PropertyInfo info => info.GetValue(objectValue, null),
                _ => throw new NotSupported("asd")
            } ?? throw new NotFound("asd");

            if (value.GetType().IsArray)
                return CommandQueryFragment.Merge(null, ReturnType.Record, columnCommandQueryPart, CommandQueryFragment.Create(new ColumnPropertyCollection(value as IEnumerable<object>)));

            foreach (var column in columnProperties)
            {
                var isDefault = false;
                var dynamicValue = Dynamic.InvokeGet(value, column.ColumnPropertyInfo?.Name);
                if (dynamicValue == null)
                {
                    var columnDefaultValue = column.ColumnPropertyInfo?.DefaultValue;
                    if (column.ColumnPropertyInfo?.DefaultValue != null)
                    {
                        isDefault = true;
                        dynamicValue = columnDefaultValue;
                    }
                }
                GetOption("Command", out var command);
                 if (command is null || (command.ToString() != "Edit" && (column.ColumnPropertyInfo?.Required ?? false) && dynamicValue is null && column.ColumnPropertyInfo.DefaultValue is null))
                    throw new ArgumentNullException($"Value of Property:{column.ColumnPropertyInfo?.Name} can not be null.");

                if (dynamicValue == null)
                {
                    column.SetValue(null);
                    continue;
                }

                if (column.ColumnPropertyInfo?.Type.GetInterface(nameof(IQueryableObject)) != null)
                {
                    var cacheObject = CacheObjectCollection.LastDatabaseObjectInfo(column.ColumnPropertyInfo?.Type!) ?? throw new System.Exception(); //todo
                    var key = cacheObject?.PropertyInfos.FirstOrDefault(item => item.Key) ?? throw new System.Exception(); //todo
                    object? objectColumnValue = isDefault ? dynamicValue : Dynamic.InvokeGet(dynamicValue, key.Name);
                    column.SetValue(objectColumnValue);
                }
                else
                {
                    column.SetValue(dynamicValue);
                }
            }

            return CommandQueryFragment.Create(columnProperties);
        }

        throw new System.Exception(); //todo
    }

    protected override CommandQueryFragment VisitConstant(ConstantExpression expression, string? memberName = null, MemberExpression? memberExpression = null)
    {
        var value = expression.ObjectValue() ?? throw new NotSupported("easd"); //todo

        return CommandQueryFragment.Create(new ColumnPropertyCollection(new[] {value}));
    }
}
