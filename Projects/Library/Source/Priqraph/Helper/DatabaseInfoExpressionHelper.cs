using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Exception;

namespace Priqraph.Helper;

internal static class DatabaseInfoExpressionHelper
{
    public static bool TryExpandProperty(this IColumnPropertyInfo parent, ICacheInfoCollection cacheInfoCollection, out ICollection<IColumnPropertyInfo>? expandedProperties)
    {
        //ToDo : نباید System.Type  را بیاورد
        expandedProperties = null;
        if (!cacheInfoCollection.TryGetLastDatabaseObjectInfo(parent.Type, out var objectInfo))
            return false;


        if (objectInfo == null)
            throw new NotFoundException("ObjectInfo", parent.Name, ExceptionCode.DatabaseQueryGeneratorGetProperty);

        var properties = objectInfo.PropertyInfos.Where(item => item.FieldType != DatabaseFieldType.Related).ToArray();
        expandedProperties = properties.Properties(parent, !(parent.Parent == null && objectInfo.DataSet == parent.Name)).ToList();

        return expandedProperties.Count > 0;
    }
}
