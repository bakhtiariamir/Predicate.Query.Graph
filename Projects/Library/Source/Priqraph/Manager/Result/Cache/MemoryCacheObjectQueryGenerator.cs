using Priqraph.Builder.Cache;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;

namespace Priqraph.Manager.Result.Cache
{
    public class MemoryCacheObjectQueryGenerator : IObjectQueryGenerator<BaseQueryParameter, MemoryCacheObjectQuery, CacheQueryResult>
    {
        public MemoryCacheObjectQuery? GenerateResult(DatabaseQueryOperationType operationType, CacheQueryResult query)
        {
            var cacheQuery = new CacheQueryObject();
            switch (operationType)
            {
                case DatabaseQueryOperationType.GetData:
                    cacheQuery = query.GenerateSelect();
                    break;
                case DatabaseQueryOperationType.Add:
                case DatabaseQueryOperationType.Edit:
                case DatabaseQueryOperationType.Remove:
                case DatabaseQueryOperationType.Merge:
                    cacheQuery = query.GenerateCommandQuery();
                    break;
            }

            return new MemoryCacheObjectQuery(cacheQuery, new List<BaseQueryParameter>());
        }
    }
}
