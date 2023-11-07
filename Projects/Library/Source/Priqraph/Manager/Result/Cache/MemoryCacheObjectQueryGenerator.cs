using Priqraph.Builder.Cache;
using Priqraph.Contract;
using Priqraph.DataType;
using Priqraph.Helper;

namespace Priqraph.Manager.Result.Cache
{
    public class MemoryCacheObjectQueryGenerator : IObjectQueryGenerator<BaseQueryParameter, MemoryCacheObjectQuery, CacheQueryResult>
    {
        public MemoryCacheObjectQuery? GenerateResult(QueryOperationType operationType, CacheQueryResult query)
        {
            var cacheQuery = new CacheQueryObject();
            switch (operationType)
            {
                case QueryOperationType.GetData:
                    cacheQuery = query.GenerateSelect();
                    break;
                case QueryOperationType.Add:
                case QueryOperationType.Edit:
                case QueryOperationType.Remove:
                case QueryOperationType.Merge:
                    cacheQuery = query.GenerateCommandQuery();
                    break;
            }

            return new MemoryCacheObjectQuery(cacheQuery, new List<BaseQueryParameter>());
        }
    }
}
